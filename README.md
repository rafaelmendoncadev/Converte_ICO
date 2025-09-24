# Conversor de ICO

Um aplicativo web ASP.NET Core MVC para converter imagens (PNG, JPG, JPEG, SVG) para o formato .ICO com múltiplas resoluções.

## ?? Funcionalidades

- **Upload de Imagens**: Suporte para PNG, JPG, JPEG e SVG (até 10MB)
- **Preview de Imagem**: Visualização da imagem antes da conversão
- **Múltiplas Resoluções**: Conversão para 16x16, 32x32, 64x64 e 128x128 pixels
- **Download Direto**: Download do arquivo .ICO gerado
- **Limpeza Automática**: Remoção automática de arquivos temporários após 2 horas
- **Interface Responsiva**: Design moderno com Bootstrap e FontAwesome

## ??? Tecnologias Utilizadas

- **Framework**: ASP.NET Core MVC (.NET 8)
- **Processamento de Imagens**: Magick.NET
- **Frontend**: Bootstrap 5 + FontAwesome 6
- **Sessões**: ASP.NET Core Sessions para manter estado

## ?? Instalação e Execução

1. Clone o repositório
2. Navegue até o diretório do projeto
3. Execute o comando:
   ```bash
   dotnet run
   ```
4. Acesse `http://localhost:5179` no seu navegador

## ?? Configuração

O projeto está configurado para:
- Executar na porta 5179
- Limpar arquivos temporários a cada hora
- Manter sessões por 30 minutos
- Aceitar arquivos de até 10MB

## ?? Estrutura do Projeto

```
??? Controllers/
?   ??? HomeController.cs          # Controller principal
??? Models/
?   ??? ImageUploadViewModel.cs    # Modelo para upload
??? Services/
?   ??? ImageConverterService.cs   # Serviço de conversão
??? BackgroundServices/
?   ??? TempFileCleanupService.cs  # Limpeza automática
??? Views/
?   ??? Home/
?   ?   ??? Index.cshtml          # Página principal
?   ?   ??? Privacy.cshtml        # Política de privacidade
?   ??? Shared/
?       ??? _Layout.cshtml        # Layout principal
??? wwwroot/
    ??? css/site.css              # Estilos customizados
    ??? temp/                     # Arquivos temporários
```

## ?? Como Usar

1. **Upload**: Selecione uma imagem nos formatos suportados
2. **Preview**: Visualize a imagem carregada
3. **Configuração**: Escolha os tamanhos desejados (16x16, 32x32, 64x64, 128x128)
4. **Nome**: Defina um nome personalizado para o arquivo
5. **Conversão**: Clique em "Converter para ICO"
6. **Download**: Baixe o arquivo .ICO gerado

## ?? Privacidade e Segurança

- Todos os arquivos são processados localmente
- Remoção automática de arquivos temporários após 2 horas
- Não há coleta de dados pessoais
- Conexão segura (HTTPS)
- Validação rigorosa de tipos de arquivo

## ?? Licença

Este projeto é uma ferramenta gratuita e de código aberto para conversão de imagens.

## ?? Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.