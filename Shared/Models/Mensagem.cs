using System;

namespace ChatSimulador.Models
{
    public class Mensagem
    {
        // Propriedades existentes (baseadas nas suas 15 colunas e necessidades)
        public string Plataforma { get; set; }
        public DateTime DataHora { get; set; }
        public string Remetente { get; set; }
        public string NomeExibicao { get; set; }
        public string Avatar { get; set; } // Caminho para o avatar
        public string TipoMensagem { get; set; } // Ex: "texto", "imagem", "audio", "figurinha", "chamada"
        public string Conteudo { get; set; } // Texto da mensagem ou legenda da mídia
        public string Midia { get; set; } // Caminho para o arquivo de mídia (imagem, audio, figurinha)
        public StatusLeitura StatusLeitura { get; set; } // Usando o enum StatusLeitura
        public TimeSpan Duracao { get; set; } // Para áudios ou chamadas
        public string ColunaAdicional1 { get; set; } // Ex: ID da mensagem, etc.
        public string ColunaAdicional2 { get; set; } // Ex: Tipo de chamada (perdida, recebida), etc.

        // --- Propriedades que estavam faltando e gerando CS1061 ---

        // Propriedade para o balão de data (ex: "Hoje", "Ontem", "30 de setembro de 2025")
        // Esta propriedade será populada na lógica de exibição ou no PageModel
        public string DataBalao { get; set; }

        // Indica se a mensagem é uma chamada (para ícones específicos)
        public bool IsChamada => TipoMensagem?.ToLower() == "chamada";

        // Indica se a mensagem deve ser exibida borrada (opcional, para privacidade)
        public bool Borrado { get; set; } = false;

        // Propriedade para reações (ex: emoji de reação a uma mensagem)
        public string Reacao { get; set; } // Ex: "👍", "❤️", etc.

        // Indica se a mensagem pertence a um grupo
        public bool IsGrupo { get; set; } = false; // Pode ser determinado pelo Remetente ou uma coluna específica

        // --- Propriedades adicionais úteis ---

        // Indica se a mensagem foi enviada pelo usuário logado (para alinhar balões)
        public bool IsMinhaMensagem { get; set; }

        // Indica se a mensagem foi apagada
        public bool IsApagada => StatusLeitura == StatusLeitura.Apagada;
    }
}
