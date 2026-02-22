using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [Header("Velocidades")]
    public float velocidadeNormal = 3f;
    public float velocidadeAcelerada = 5.5f;
    public float velocidadeLento = 1.5f; 
    public float velocidadeFuga = 6f;    

    [Header("Configurações Gerais")]
    public Transform[] pontosPatrulha;
    public PlayerController player;
    public float distanciaInvestigacao = 6f; 

    [Header("Tempos de Espera e Cooldown")]
    public float tempoEsperaPatrulha = 2f;     
    public float tempoEsperaInvestigacao = 3f; 
    public float tempoCooldownFuga = 3f; // Tempo ignorando o player após fugir

    [Header("Resistência e Fuga")]
    public float tempoParaCegar = 1.5f; 
    public float tempoDeFuga = 2f;
    
    private float timerFuga = 0f;
    private float timerCooldown = 0f; 
    private float timerCegando = 0f;
    private float velocidadeAtual;
    private float cronometro = 0f;
    private bool esperando = false;
    private int pontoAtual = 0;
    private Vector2 ultimoLocalConhecido;
    private bool investigando = false;
    private bool estaAcelerado = false;

    void Start()
    {
        velocidadeAtual = velocidadeNormal;
    }

    void Update()
    {
        if (PauseMenu.isPaused) return;

        // Reduz o cooldown se houver
        if (timerCooldown > 0) timerCooldown -= Time.deltaTime;

        velocidadeAtual = estaAcelerado ? velocidadeAcelerada : velocidadeNormal;

        // 1. ESTADO DE FUGA
        if (timerFuga > 0)
        {
            timerFuga -= Time.deltaTime;
            velocidadeAtual = velocidadeFuga;
            
            if (pontosPatrulha.Length > 0)
            {
                Transform alvoFuga = pontosPatrulha[pontoAtual];
                MoverPara(alvoFuga.position);
                
                if (Vector2.Distance(transform.position, alvoFuga.position) < 0.1f)
                    pontoAtual = (pontoAtual + 1) % pontosPatrulha.Length;
            }

            // Inicia o cooldown no momento em que a fuga acaba
            if (timerFuga <= 0) timerCooldown = tempoCooldownFuga;
            return; 
        }

        // 2. LANTERNA LIGADA (Sem limite de visão, apenas cooldown)
        if (player != null && player.isFlashlightOn && timerCooldown <= 0)
        {
            float distanciaPlayer = Vector2.Distance(transform.position, player.transform.position);
            bool luzNaCara = player.EstaIluminando(transform.position);

            if (luzNaCara && player.isFocused)
            {
                velocidadeAtual = velocidadeLento; 
                timerCegando += Time.deltaTime; 
                
                if (timerCegando >= tempoParaCegar)
                {
                    timerFuga = tempoDeFuga; 
                    estaAcelerado = false;
                    investigando = false;
                    timerCegando = 0f; 
                    return;
                }
            }
            else timerCegando = 0f; 
            
            investigando = true;
            esperando = false; 
            ultimoLocalConhecido = player.transform.position;

            if (!(luzNaCara && player.isFocused))
            {
                if (distanciaPlayer > player.normalRadius && distanciaPlayer <= player.focusedRadius)
                {
                    estaAcelerado = true;
                    velocidadeAtual = velocidadeAcelerada;
                }
            }

            MoverPara(player.transform.position);
            return;
        }
        else
        {
            timerCegando = 0f; 
        }

        // 3. Sistema de Espera
        if (esperando)
        {
            cronometro -= Time.deltaTime;
            if (cronometro <= 0f)
            {
                esperando = false;
                if (investigando) { investigando = false; estaAcelerado = false; }
                else pontoAtual = (pontoAtual + 1) % pontosPatrulha.Length; 
            }
            return; 
        }

        // 4. Investigação
        if (investigando)
        {
            if (Vector2.Distance(transform.position, ultimoLocalConhecido) > distanciaInvestigacao)
            {
                investigando = false; estaAcelerado = false; 
            }
            else
            {
                MoverPara(ultimoLocalConhecido);
                if (Vector2.Distance(transform.position, ultimoLocalConhecido) < 0.1f)
                    IniciarEspera(tempoEsperaInvestigacao); 
            }
        }
        // 5. Patrulha
        else if (pontosPatrulha.Length > 0)
        {
            Transform alvoPatrulha = pontosPatrulha[pontoAtual];
            MoverPara(alvoPatrulha.position);

            if (Vector2.Distance(transform.position, alvoPatrulha.position) < 0.1f)
                IniciarEspera(tempoEsperaPatrulha); 
        }
    }

    void IniciarEspera(float tempo)
    {
        esperando = true;
        cronometro = tempo;
    }

    void MoverPara(Vector2 destino)
    {
        Vector2 direcao = destino - (Vector2)transform.position;
        if (direcao != Vector2.zero) transform.up = direcao; 
        transform.position = Vector2.MoveTowards(transform.position, destino, velocidadeAtual * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Só mata se o cooldown estiver zerado
        if (collision.gameObject.CompareTag("Player") && timerCooldown <= 0) player.Morrer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Só mata se o cooldown estiver zerado
        if (collision.CompareTag("Player") && timerCooldown <= 0) player.Morrer();
    }
}