# Tecnologias
- .Net Core 5

# Packages
- Newtonsoft.Json
- MySqlConnector
- AWSSDK.SQS
- AWSSDK.Extensions.NETCore.Setup
- Swashbuckle.AspNetCore

# Observações
- Gostaria de me desculpar antecipadamente pela demora na entrega e pela entrega incompleta, contudo segue meus apontamentos
- O repositório tem uma implementação breve de como seria a lógica solicitada à ser implementada por email (_**Teste 1**_)
- A implementação de verificação das filas de processamento por parte da AWSSQS ficaram apenas abstraídas por necessitarem de um registro válido no site da AWS
- O modelo de implementação que ficou abstraído dentro do código fonte foi retirado do site (https://www.c-sharpcorner.com/article/how-to-implement-amazon-sqs-aws-sqs-in-asp-net-core-project/)
- Ressalvo que o ambiente cloud da AWS não é minha especialidade e pendência de autenticação de usuário na AWS foi o fator crucial para não ter sido implementado.
- Interface de interação com a API via Swagger
- A conexão com a base de dados MYSQL é fornecida por um dos meus servidores da Oracle Cloud (**_Peço a gentileza de que somente seja utilizado como fins de verificação de registros gerados pela própria API_**)
- O banco de dados pode ser acessado através da URL: http://144.22.210.70/phpmyadmin, Login: shared, Senha: C&!@Q$.4#]A9m,V6%$m(G2udGgfHFj-q)4pZ$@NR5
