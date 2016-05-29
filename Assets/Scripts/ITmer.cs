/// <summary>
/// Controle de temporzadores
/// </summary>
public interface ITimer
{

    /// <summary>
    /// Inicia a Contagem
    /// </summary>
    void Play();
    /// <summary>
    /// Puasa a contagem
    /// </summary>
    void Pause();
    /// <summary>
    /// Continua a contagem
    /// </summary>
    void Resume();
    /// <summary>
    /// Para e reseta a contagem
    /// </summary>
    void Stop();
    
}
