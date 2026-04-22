namespace ChatSimulador.Shared.Models
{
    public enum StatusLeitura
    {
        // Mensagem enviada, mas não entregue
        Enviada,
        // Mensagem entregue, mas não lida
        Entregue,
        // Mensagem lida
        Lida,
        // Mensagem não enviada (erro ou rascunho)
        NaoEnviada,
        // Mensagem apagada (não é um status de leitura, mas um estado da mensagem)
        Apagada
    }
}