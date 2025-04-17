# GrupoColorado

Composição da solução:<br />

<b>Database</b>: SQL Server Express 2022<br />
<b>Backend</b>: ASP.NET WebAPI com .NET 6.0 e C#<br />
<b>Frontend</b>: ASP.NET WebApp MVC com .NET 6.0, C# e jQuery<br />


## Procedimentos para instalação

1. Clone o repositório:<br />
  <b>git clone https://github.com/arsmmd/GrupoColorado.git</b><br />

2. Navegue até a raiz do diretório clonado:<br />
  <b>cd .\GrupoColorado\ </b><br />

3. Navegue até o diretório DatabaseScripts:<br />
  <b>cd .\DatabaseScripts\ </b><br />

4. Execute o script <b>Install.sql</b> em uma instância do SQL Server;<br />

5. Navegue até a raiz do diretório clonado:<br />
  <b>cd .\GrupoColorado\ </b><br />

6. Abra o arquivo da solução (GrupoColorado.sln) no Visual Studio;<br />

7. Ajuste a string de conexão no appSettings.json do projeto backend.<br />

8. Quando tudo estiver configurado, o projeto pode ser executado e a tela de Login será exibida. O usuário de entrada está sendo inserido no script da base de dados.<br />