using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    public virtual UnitType unitType { get; set; }

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private SpriteLibrary spriteLibrary;

    [SerializeField] private Animator animator;

    [Header("Unit Info")]
    public float maxHp;
    public float atkPower;
    public float atkDelay;
    public int atkRange;
    public float moveSpeed = 0.5f;

    [HideInInspector] public float currentHp;
    [HideInInspector] public Unit currentTarget = null;
    [HideInInspector] public UnitState unitState = UnitState.Dead;

    [SerializeField] private Slider hpBar;

    public virtual void Init()
    {
        currentHp = this.maxHp;
        hpBar.value = this.currentHp / this.maxHp;
        this.gameObject.SetActive(true);

        unitState = UnitState.Live;
        DoAnimation(AnimationType.Idle);
        StartCoroutine(TrackCoroutine());
    }
    public virtual void SetData(UnitData _data)
    {
        unitType = _data.unitType;
        maxHp = _data.maxHp;
        atkPower = _data.atkPower;
        atkDelay = _data.atkDelay;
        atkRange = _data.atkRange;
        moveSpeed = _data.moveSpeed;

        sprite.sprite = _data.sprite;
        spriteLibrary.spriteLibraryAsset = _data.spriteLibrary;
    }

    public void DoAnimation(AnimationType _type)
    {
        if (gameObject.activeSelf == false)
            return;

        animator.speed = GameManager.instance.gameSpeed;

        switch (_type)
        {
            case AnimationType.Idle:
                animator.Play("Idle");
                break;
            case AnimationType.Move:
                animator.Play("Move");
                break;
            case AnimationType.Atk:
                animator.Play("Attack");
                break;
            case AnimationType.Death:
                animator.Play("Death");
                break;
        }
    }
    public Unit GetTarget(Unit _unit)
    {
        Unit target = null;
        float distanceMax = float.MaxValue;

        Transform _targets = _unit.unitType == UnitType.Character ? 
            GameManager.instance.monsters : GameManager.instance.characters;

        for (int i = 0; i < _targets.childCount; i++)
        {
            if (_targets.GetChild(i).GetComponent<Unit>().unitState == UnitState.Live)
            {
                float curDistance = Vector2.Distance(_unit.transform.position, _targets.GetChild(i).transform.position);
                if (curDistance < distanceMax)
                {
                    target = _targets.GetChild(i).GetComponent<Unit>();
                    distanceMax = Vector2.Distance(_unit.transform.position, _targets.GetChild(i).transform.position);
                }
            }
        }
        return target;
    }
    #region Coroutine
    public IEnumerator TrackCoroutine()
    {
        yield return new WaitForFixedUpdate();
        StartCoroutine(MoveCoroutine());
    }
    public IEnumerator MoveCoroutine()
    {
        currentTarget = GetTarget(this);

        if (currentTarget == null)
        {
            DoAnimation(AnimationType.Idle);
            yield return new WaitForFixedUpdate();
            StartCoroutine(TrackCoroutine());
        }
        else
        {
            SetDirection(this.transform.localPosition.x < currentTarget.transform.localPosition.x);
            float curDistance = Vector2.Distance(this.transform.position, currentTarget.transform.position);

            if (curDistance < atkRange)
            {
                StartCoroutine(AttackCoroutine());
            }
            else
            {
                DoMove(this.transform.position, currentTarget.transform.position);
                yield return new WaitForFixedUpdate();
                StartCoroutine(TrackCoroutine());
            }
        }
    }
    public IEnumerator AttackCoroutine()
    {
        DoAttack(currentTarget);
        yield return new WaitForSecondsRealtime(atkDelay / GameManager.instance.gameSpeed);
        DoAnimation(AnimationType.Idle);
        StartCoroutine(TrackCoroutine());
    }
    public void StopUnitCoroutines()
    {
        StopAllCoroutines();
    }
    #endregion
    #region Action
    public void SetDirection(bool _isRight)
    {
        sprite.flipX = !_isRight;
    }
    public void DoMove(Vector3 _unitPos, Vector3 _targetPos)
    {
        DoAnimation(AnimationType.Move);
        this.transform.position = Vector3.MoveTowards(_unitPos, _targetPos, Time.deltaTime * moveSpeed * GameManager.instance.gameSpeed);
    }
    public void DoAttack(Unit _target)
    {
        DoAnimation(AnimationType.Atk);
        DoDamage(_target, this.atkPower);
    }
    public virtual void DoDamage(Unit _target, float _value)
    {
        _target.currentHp = _target.currentHp - _value > 0.0f ? _target.currentHp - _value : 0.0f;
        _target.hpBar.value = _target.currentHp / _target.maxHp;
        if (_target.currentHp == 0)
        {
            _target.Die();
        }
    }
    public virtual void Die()
    {
        StopUnitCoroutines();
        DoAnimation(AnimationType.Death);
        unitState = UnitState.Dead;
    }
    #endregion
}