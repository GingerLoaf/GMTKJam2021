using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showcase : MonoBehaviour {

    public Texture texWhite;
    public Texture texRed;
    public Texture texGreen;
    public Texture texBlue;
    public Renderer rend;

    public Scrollbar scrollBar;
    public Animator character;

    public bool isWalking = false;
    public bool isResting = false;
    public bool isAttacking = false;
    public Toggle walkLoop;
    public Toggle rest;
    public Toggle attackLoop;
    public Button attack;
    public Button takeDamage;
    public Button taunt;
    public Button throwButton;
    public Button dieButton;

    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(transform.rotation.x, 360*scrollBar.value, transform.rotation.z);

        if (isAttacking)
        {
            attack.interactable = true;
            taunt.interactable = true;
            takeDamage.interactable = true;

            throwButton.interactable = false;
            dieButton.interactable = false;
        }
        else {
            attack.interactable = false;
            taunt.interactable = false;
            takeDamage.interactable = false;

            throwButton.interactable = true;
            dieButton.interactable = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();

        }

	}

    //animations

    public void ToggleWalk() {
        isWalking = !isWalking;
        character.SetBool("isWalking", isWalking);
        walkLoop.isOn = isWalking;
    }

    public void ToggleRest()
    {
        Halt();
        isResting = !isResting;
        character.SetBool("isResting", isResting);
        rest.isOn = isResting;
    }

    public void Halt() {
        walkLoop.isOn = false;
        character.SetBool("isWalking", false);
        attackLoop.isOn = false;
        character.SetBool("isAttacking", false);
    }

    public void AttackPose()
    {
        isAttacking = !isAttacking;
        character.SetBool("isAttacking", isAttacking);
        attackLoop.isOn = isAttacking;
    }

    public void Attack() {        
        character.SetTrigger("attack");
    }

    public void TakeDamage()
    {
        character.SetTrigger("takeDamage");
    }

    public void Taunt()
    {
        character.SetTrigger("taunt");
    }

    public void Throw()
    {
        character.SetTrigger("throw");
    }

    public void Die()
    {
        character.SetTrigger("die");
    }

    public void SetWhiteTexture() {
        rend.sharedMaterial.SetTexture("_MainTex", texWhite);
    }
    public void SetRedTexture()
    {
        rend.sharedMaterial.SetTexture("_MainTex", texRed);
    }
    public void SetGreenTexture()
    {
        rend.sharedMaterial.SetTexture("_MainTex", texGreen);
    }
    public void SetBlueTexture()
    {
        rend.sharedMaterial.SetTexture("_MainTex", texBlue);
    }

}
