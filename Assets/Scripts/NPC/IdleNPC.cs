using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNPC : Sign
{
    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private Animator anim;

    public string[] dialogues; // Array untuk menyimpan daftar dialog
    private int currentDialogueIndex = 0; // Indeks dialog saat ini yang akan ditampilkan

    private bool canInteract = true; // Mengontrol apakah pemain dapat berinteraksi lagi

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("interact") && playerInRange && canInteract)
        {
            ShowNextDialog();
        }
    }

    // Menampilkan dialog berikutnya dalam array dialogues
    private void ShowNextDialog()
    {
        if (currentDialogueIndex < dialogues.Length)
        {
            dialogBox.SetActive(true);
            dialogText.text = dialogues[currentDialogueIndex];
            nameText.text = nama;
            currentDialogueIndex++;
        }
        else
        {
            // Jika dialog telah habis, tutup dialog box dan reset indeks
            dialogBox.SetActive(false);
            currentDialogueIndex = 0;
            canInteract = false; // Tidak dapat berinteraksi lagi setelah dialog selesai
        }
    }

    // Override method untuk mengatur ulang keadaan interaksi saat pemain keluar dari jangkauan
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = false;
            dialogBox.SetActive(false);
            currentDialogueIndex = 0;
            canInteract = true; // Mengizinkan berinteraksi lagi saat pemain kembali dalam jangkauan
        }
    }
}
