using UnityEngine;
public class Upgrade : MonoBehaviour
{
	private MetaUiManager ui;
    public float timer;
    public SpriteRenderer loader;
    public static bool upGradePanelActivated;

	private void Start()
    {
        ui = MetaUiManager.instance;
        timer = loader.material.GetFloat("_Arc1");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!upGradePanelActivated)
            {
                if (timer <= 0)
                {
                    ui.upGradePanel.SetActive(true);
                    upGradePanelActivated = true;
                    timer = 360f;
                    loader.material.SetFloat("_Arc1", timer);
                }
                else
                {
                    timer -= (360f) * Time.deltaTime;
                    loader.material.SetFloat("_Arc1", timer);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            upGradePanelActivated = false;
            timer = 360f;
            loader.material.SetFloat("_Arc1", timer);
        }
    }
}