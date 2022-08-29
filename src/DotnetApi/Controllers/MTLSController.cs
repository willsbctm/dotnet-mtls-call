using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace DotnetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MTLSController : ControllerBase
{
    private readonly ILogger<MTLSController> _logger;

    public MTLSController(ILogger<MTLSController> logger)
    {
        _logger = logger;
    }

    [HttpPost()]
    public async Task<ActionResult> GetAsync([FromBody]Requisicao requisicao)
    {
        var certificado = X509Certificate2.CreateFromPemFile(requisicao.Certificado, requisicao.Chave);
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ClientCertificates.Add(certificado);
        //handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var client = new HttpClient(handler);
        var resultado = await client.GetAsync(requisicao.Url);
        var conteudo = await resultado.Content.ReadAsStringAsync();
        return Ok(new { conteudo, resultado});
    }

    public class Requisicao {
      public string Certificado { get; set; }
      public string Chave { get; set; }
      public string Url { get; set; }
    }
}
