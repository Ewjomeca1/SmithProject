using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform materialHolder;
    [SerializeField]private CraftingMaterial holdingMaterial;
    [SerializeField] private MaterialManipulator materialManipulator;

    public float speed = 5;
    
    void Update()
    {
        Move();
        if(Input.GetKeyDown(KeyCode.F)) DiscardMaterial();
        if (Input.GetKeyDown(KeyCode.E) && materialManipulator)
        {         
                if (IsHoldingMaterial() && materialManipulator.CanHoldMaterial())
                {
                    materialManipulator.HoldMaterial(RemoveMaterial());
                    return;
                }
                if (!IsHoldingMaterial() && materialManipulator.CanRemoveMaterial())
                {
                    HoldMaterial(materialManipulator.RemoveMaterial());
                    return;
                }
            
        }               
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MaterialManipulator materialManipulator)) this.materialManipulator = materialManipulator;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MaterialManipulator materialManipulator)) this.materialManipulator = null;
    }

    private void Move()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if(input.magnitude > 0)
        {
            transform.forward = input;
            transform.position += transform.forward * input.magnitude * speed * Time.deltaTime;
        }
    }    

    public void HoldMaterial(CraftingMaterial material) 
    {
        if(!holdingMaterial && material)
        {
            holdingMaterial = material;
            holdingMaterial.transform.parent = materialHolder;
            holdingMaterial.transform.localPosition = Vector3.zero;
            holdingMaterial.transform.localRotation = Quaternion.identity;
        }
    }

    public CraftingMaterial RemoveMaterial()
    {
        CraftingMaterial material = holdingMaterial;
        holdingMaterial.transform.SetParent(null);
        holdingMaterial = null;
        return material;
    }
    public bool IsHoldingMaterial() => holdingMaterial;


    public void DiscardMaterial()
    {
        if (holdingMaterial)
        {
            Destroy(holdingMaterial.gameObject);
            holdingMaterial = null;
        }
    }
}