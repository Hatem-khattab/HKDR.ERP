using System.Net.Http.Headers;


namespace HKDR.UI.Services.Auth
{

    public class JwtHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _context;

        public JwtHandler(IHttpContextAccessor context)
        {
            _context = context;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = _context.HttpContext?.Session.GetString("JWT");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }

}
