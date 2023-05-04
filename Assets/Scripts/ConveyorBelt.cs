using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private Animator conveyorAnime;

    void Start()
    {
        conveyorAnime.Play("ConveyerBelt", 0, 0.0f);
    }
}