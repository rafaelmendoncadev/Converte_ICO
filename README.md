# Conversor de ICO

Um aplicativo web ASP.NET Core MVC para converter imagens (PNG, JPG, JPEG, SVG) para o formato .ICO com m�ltiplas resolu��es.

## ?? Funcionalidades

- **Upload de Imagens**: Suporte para PNG, JPG, JPEG e SVG (at� 10MB)
- **Preview de Imagem**: Visualiza��o da imagem antes da convers�o
- **M�ltiplas Resolu��es**: Convers�o para 16x16, 32x32, 64x64 e 128x128 pixels
- **Download Direto**: Download do arquivo .ICO gerado
- **Limpeza Autom�tica**: Remo��o autom�tica de arquivos tempor�rios ap�s 2 horas
- **Interface Responsiva**: Design moderno com Bootstrap e FontAwesome

## ??? Tecnologias Utilizadas

- **Framework**: ASP.NET Core MVC (.NET 8)
- **Processamento de Imagens**: Magick.NET
- **Frontend**: Bootstrap 5 + FontAwesome 6
- **Sess�es**: ASP.NET Core Sessions para manter estado

## ?? Instala��o e Execu��o

1. Clone o reposit�rio
2. Navegue at� o diret�rio do projeto
3. Execute o comando:
   ```bash
   dotnet run
   ```
4. Acesse `http://localhost:5179` no seu navegador

## ?? Configura��o

O projeto est� configurado para:
- Executar na porta 5179
- Limpar arquivos tempor�rios a cada hora
- Manter sess�es por 30 minutos
- Aceitar arquivos de at� 10MB

## ?? Estrutura do Projeto

```
??? Controllers/
?   ??? HomeController.cs          # Controller principal
??? Models/
?   ??? ImageUploadViewModel.cs    # Modelo para upload
??? Services/
?   ??? ImageConverterService.cs   # Servi�o de convers�o
??? BackgroundServices/
?   ??? TempFileCleanupService.cs  # Limpeza autom�tica
??? Views/
?   ??? Home/
?   ?   ??? Index.cshtml          # P�gina principal
?   ?   ??? Privacy.cshtml        # Pol�tica de privacidade
?   ??? Shared/
?       ??? _Layout.cshtml        # Layout principal
??? wwwroot/
    ??? css/site.css              # Estilos customizados
    ??? temp/                     # Arquivos tempor�rios
```

## ?? Como Usar

1. **Upload**: Selecione uma imagem nos formatos suportados
2. **Preview**: Visualize a imagem carregada
3. **Configura��o**: Escolha os tamanhos desejados (16x16, 32x32, 64x64, 128x128)
4. **Nome**: Defina um nome personalizado para o arquivo
5. **Convers�o**: Clique em "Converter para ICO"
6. **Download**: Baixe o arquivo .ICO gerado

## ?? Privacidade e Seguran�a

- Todos os arquivos s�o processados localmente
- Remo��o autom�tica de arquivos tempor�rios ap�s 2 horas
- N�o h� coleta de dados pessoais
- Conex�o segura (HTTPS)
- Valida��o rigorosa de tipos de arquivo

## ?? Licen�a

Este projeto � uma ferramenta gratuita e de c�digo aberto para convers�o de imagens.

## ?? Contribui��es

Contribui��es s�o bem-vindas! Sinta-se � vontade para abrir issues ou pull requests.