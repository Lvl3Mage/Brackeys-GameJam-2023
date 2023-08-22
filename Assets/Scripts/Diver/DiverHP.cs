using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiverHP : MonoBehaviour
{
    [SerializeField] private float diverHp;
    //[SerializeField] private Slider hpBar;

    public void Damage(int hp)
    {
        diverHp -= hp;
        //hpBar.value = diverHp;
    }
}
