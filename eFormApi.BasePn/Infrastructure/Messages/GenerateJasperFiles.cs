﻿namespace Microting.eFormApi.BasePn.Infrastructure.Messages
{
    public class GenerateJasperFiles
    {
        public int TemplateId { get; protected set; }

        public GenerateJasperFiles(int templateId)
        {
            TemplateId = templateId;
        }
    }
}