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
    [SerializeField] private Slider hpBar;

    public float maxHp { get; protected set; }
    public float atkPower { get; protected set; }
    public float atkDelay { get; private set; }
    public float moveSpeed { get; protected set; } = 0.5f;
    public float currentHp { get; protected set; }
    public int atkRange { get; private set; }
    public Unit currentTarget { get; protected set; } = null;
    public UnitState unitState { get; protected set; } = UnitState.Dead;
    public AnimationType animationType{ get; protected set; } = AnimationType.Idle;

    private float trackDelay = 0.002f;
    private float currnetDelay = 0f;
    private bool canAttack = true;

    private void Update()
    {
        if (currentHp <= 0 || unitState == UnitState.Dead)
            return;

        currnetDelay += Time.deltaTime;
        if(currnetDelay >= trackDelay)
        {
            currnetDelay = 0.0f;
            currentTarget = GetTarget(this);

            if (currentTarget == null)
            {
                DoAnimation(AnimationType.Idle);
            }
            else
            {
                SetDirection(this.transform.localPosition.x < currentTarget.transform.localPosition.x);
                float curDistance = Vector2.Distance(this.transform.position, currentTarget.transform.position);

                if (curDistance < atkRange)
                {
                    if (!canAttack)
                    {
                        DoAnimation(AnimationType.Idle);
                        return;
                    }
                    DoAttack(currentTarget);
                }
                else
                {
                    DoMove(this.transform.position, currentTarget.transform.position);
                }
            }
        }
    }
    public virtual void Init()
    {
        currentHp = this.maxHp;
        hpBar.value = this.currentHp / this.maxHp;
        this.gameObject.SetActive(true);

        unitState = UnitState.Live;
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

        if (animationType == _type)
            return;

        switch (_type)
        {
            case AnimationType.Idle:
                {
                    animationType = AnimationType.Idle;
                    animator.SetTrigger("Idle");
                    break;
                }
            case AnimationType.Move:
                {
                    animationType = AnimationType.Move;
                    animator.SetTrigger("Move");
                    break;
                }
            case AnimationType.Atk:
                {
                    animationType = AnimationType.Atk;
                    animator.SetTrigger("Attack");
                    break;
                }
            case AnimationType.Death:
                {
                    animationType = AnimationType.Death;
                    animator.SetTrigger("Death");
                    break;
                }
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
    public IEnumerator AttackCoroutine()
    {
        canAttack = false;
        yield return new WaitForSecondsRealtime(atkDelay / GameManager.instance.gameSpeed);
        canAttack = true;
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
        StartCoroutine(AttackCoroutine());
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
        DoAnimation(AnimationType.Death);
        unitState = UnitState.Dead;
    }
    #endregion
}