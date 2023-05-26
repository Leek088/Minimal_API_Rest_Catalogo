namespace MinimalApiRest.AppServicesExtensions
{
    public static class ApplicationBuilderExtensions
    {
        //Habilitar o tratamento de excessões caso desenvolvedor
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app,
            IWebHostEnvironment webHostEnvironment)
        {
            // Se for desenvolvimento, habilita a pagina de excessões
            if (webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }

        // Politica de segurança Cors
        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors(p =>
            {
                //Recebe requisições de qualquer origem
                p.AllowAnyOrigin();
                //Apenas por GET
                p.WithMethods("GET");
                // Por qualquer cabeçalho
                p.AllowAnyHeader();
            });

            return app;
        }

        //Habilitar o Swagger caso desenvolvedor
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app,
            IWebHostEnvironment webHostEnvironment)
        {
            // Se for desenvolvimento, habilita a pagina de excessões
            if (webHostEnvironment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}
