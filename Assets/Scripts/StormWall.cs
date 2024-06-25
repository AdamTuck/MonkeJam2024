using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormWall : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject checkpointPos;
    [SerializeField] private float wallAdvanceSpeed;
    [SerializeField] private float healthDrainSpeed;
    [SerializeField] private GameObject playerDamageOverlay;

    private Vector3 wallStartScale;
    private MeshRenderer meshRenderer;

    bool wallAdvancing;

    public static StormWall instance;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        wallStartScale = transform.localScale;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    void Update()
    {
        if (wallAdvancing)
            AdvanceWall();
    }

    private void AdvanceWall ()
    {
        transform.localScale = new Vector3(
            transform.localScale.x - wallAdvanceSpeed * Time.deltaTime, 
            transform.localScale.y - wallAdvanceSpeed*Time.deltaTime, 
            transform.localScale.z);

        if (Vector3.Distance(player.transform.position, checkpointPos.transform.position) > transform.localScale.x/7.3)
        {
            player.GetComponent<Health>().DeductHealth(healthDrainSpeed * Time.deltaTime);
            playerDamageOverlay.SetActive(true);
        }
        else
            playerDamageOverlay.SetActive(false);

        if (transform.localScale.x < 0)
            meshRenderer.enabled = false;
    }

    public void ShrinkWall()
    {
        meshRenderer.enabled = true;
        wallAdvancing = true;
    }

    public void ResetWall()
    {
        meshRenderer.enabled = false;
        playerDamageOverlay.SetActive(false);
        wallAdvancing = false;
        transform.localScale = wallStartScale;
    }
}
