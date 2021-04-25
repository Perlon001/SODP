using Microsoft.Extensions.Configuration;
using SODP.Model;
using SODP.Model.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SODP.DataAccess
{
    public class DataInitializer : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly SODPDBContext _context;

        public DataInitializer(IConfiguration configuration, SODPDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public void DataInit()
        {
            LoadStages();
            ImportProjectFromStore();
        }

        private void LoadStages()
        {
            if(_context.Stages.Count() == 0)
            {
                string file = File.ReadAllText("generatestages.json");
                var stages = JsonSerializer.Deserialize<List<Stage>>(file);
                _context.AddRange(stages);
                //_context.Stages.Add(new Stage { Sign = "PB", Title = "PROJEKT BUDOWLANY" });
                //_context.Stages.Add(new Stage { Sign = "PBZ", Title = "PROJEKT BUDOWLANY ZAMIENNY" });
                //_context.Stages.Add(new Stage { Sign = "PBZII", Title = "PROJEKT BUDOWLANY ZAMIENNY" });
                //_context.Stages.Add(new Stage { Sign = "PAB", Title = "PROJEKT ARCHITEKTONICZNO-BUDOWLANY" });
                //_context.Stages.Add(new Stage { Sign = "PT", Title = "PROJEKT TECHNICZNY" });
                //_context.Stages.Add(new Stage { Sign = "PW", Title = "PROJEKT WYKONAWCZY" });
                //_context.Stages.Add(new Stage { Sign = "PWKS", Title = "PROJEKT WYKONAWCZY KONSTRUKCJI STALOWEJ" });
                //_context.Stages.Add(new Stage { Sign = "PK", Title = "PROJEKT KONCEPCYJNY" });
                //_context.Stages.Add(new Stage { Sign = "PR", Title = "PROJEKT ROZBIÓRKI" });
                //_context.Stages.Add(new Stage { Sign = "NI", Title = "NADZÓR INWESTORSKI" });
                //_context.Stages.Add(new Stage { Sign = "NA", Title = "NADZÓR AUTORSKI" });
                //_context.Stages.Add(new Stage { Sign = "OT", Title = "OPINIA TECHNICZNA" });
                //_context.Stages.Add(new Stage { Sign = "RE", Title = "PROJEKT REMONTU" });
                //_context.Stages.Add(new Stage { Sign = "WZ", Title = "WARUNKI ZABUDOWY" });
                _context.SaveChanges();
                //var json = JsonSerializer.Serialize(_context.Stages);
                //File.WriteAllText(@"generatestages.json", json);
            }
        }

        private void ImportProjectFromStore()
        {
            var directory = Directory.EnumerateDirectories(_configuration.GetSection("AppSettings:ProjectFolder").Value);

            foreach (var item in directory)
            {
                var localization = Path.GetFileName(item);
                var sign = localization.GetUntilOrEmpty("_");
                var currentProject = new Project()
                {
                    Number = sign.Substring(0, 4),
                    Stage = new Stage() { Sign = sign[4..] },
                    Title = localization[(sign.Length + 1)..]
                };
                var stage = _context.Stages.FirstOrDefault(x => x.Sign == currentProject.Stage.Sign);
                if (stage == null)
                {
                    stage = new Stage() { Sign = currentProject.Stage.Sign, Title = "" };
                }
                currentProject.Stage = stage;

                var project = _context.Projects.FirstOrDefault(x => x.Number == currentProject.Number && x.Stage.Sign == currentProject.Stage.Sign);
                if (project == null)
                {
                    _context.Projects.Add(currentProject);
                    _context.SaveChanges();
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
