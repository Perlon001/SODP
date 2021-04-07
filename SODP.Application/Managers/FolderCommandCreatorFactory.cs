using Microsoft.Extensions.Configuration;
using SODP.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Application.Managers
{
    public class FolderCommandCreatorFactory
    {
        private readonly IConfiguration _configuration;

        public FolderCommandCreatorFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IFolderCommandCreator GetFolderCommandCreator()
        {
            if (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT))
            {
                return new Win32FolderCommandCreator(_configuration);
            }
            else
            {
                return new UnixFolderCommandCreator(_configuration);
            }
        }
    }
}
