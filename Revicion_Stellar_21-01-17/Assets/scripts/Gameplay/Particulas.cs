using UnityEngine;
using System.Collections;

public class Particulas : MonoBehaviour {

	public Color minColor;
	public ParticleSystem particulas;
	private float Size; 
	private float tiempo;
	private Color Colorin;
	public float press = 0;
	public AudioSource audio_acelerar;
	public bool banderita = false;

	// Use this for initialization
	private void Start()
	{

		particulas = GetComponent<ParticleSystem>();
		tiempo = particulas.startLifetime;
		Size = particulas.startSize;
		Colorin = particulas.startColor;

	}


	private void Update()
	{
		if (Input.GetKey ("w") || Input.GetKey(KeyCode.UpArrow)) {
			press = press + 2.0f;

		} else
			press = 0;
		particulas.startLifetime = Mathf.Lerp(0.0f, tiempo, press * 5.0f);
		particulas.startSize = Mathf.Lerp (Size * .3f, Size, press * 5.0f);
		particulas.startColor = Color.Lerp (minColor, Colorin, press * 5.0f);
	}

}
