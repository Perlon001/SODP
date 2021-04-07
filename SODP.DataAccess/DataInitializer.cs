using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace SODP.DataAccess
{
    public class DataInitializer : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SODPDBContext _db;

        public DataInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _db = _serviceProvider.GetRequiredService<SODPDBContext>();
        }

        public void Init()
        {
            IdentityUserInit();
            LoadStages();
        }

        private void LoadStages()
        {
            if(_db.Stages.Count() == 0)
            {
                //string file = System.IO.File.ReadAllText("generatestages.json");
                //var stages = JsonSerializer.Deserialize<List<Stage>>(file);
                //_db.AddRange(stages);
                _db.Stages.Add(new Stage { Sign = "PB", Title = "PROJEKT BUDOWLANY" });
                _db.Stages.Add(new Stage { Sign = "PBZ", Title = "PROJEKT BUDOWLANY ZAMIENNY" });
                _db.Stages.Add(new Stage { Sign = "PBZII", Title = "PROJEKT BUDOWLANY ZAMIENNY" });
                _db.Stages.Add(new Stage { Sign = "PAB", Title = "PROJEKT ARCHITEKTONICZNO-BUDOWLANY" });
                _db.Stages.Add(new Stage { Sign = "PT", Title = "PROJEKT TECHNICZNY" });
                _db.Stages.Add(new Stage { Sign = "PW", Title = "PROJEKT WYKONAWCZY" });
                _db.Stages.Add(new Stage { Sign = "PWKS", Title = "PROJEKT WYKONAWCZY KONSTRUKCJI STALOWEJ" });
                _db.Stages.Add(new Stage { Sign = "PK", Title = "PROJEKT KONCEPCYJNY" });
                _db.Stages.Add(new Stage { Sign = "PR", Title = "PROJEKT ROZBIÓRKI" });
                _db.Stages.Add(new Stage { Sign = "NI", Title = "NADZÓR INWESTORSKI" });
                _db.Stages.Add(new Stage { Sign = "NA", Title = "NADZÓR AUTORSKI" });
                _db.Stages.Add(new Stage { Sign = "OT", Title = "OPINIA TECHNICZNA" });
                _db.Stages.Add(new Stage { Sign = "RE", Title = "PROJEKT REMONTU" });
                _db.Stages.Add(new Stage { Sign = "WZ", Title = "WARUNKI ZABUDOWY" });
                _db.SaveChanges();
            }
        }

        private void IdentityUserInit()
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

            CreateRoleIfNotExist(roleManager, "Administrator").Wait();
            CreateRoleIfNotExist(roleManager, "User").Wait();
            CreateRoleIfNotExist(roleManager, "ProjectManager").Wait();

            CreateUserIfNotExist(userManager, "Administrator", "Administrator").Wait();
            AddToRoleIfNotExist(userManager, "Administrator", "Administrator").Wait();

            static async Task<bool> CreateRoleIfNotExist(RoleManager<Role> roleManager, string role)
            {
                var result = await roleManager.RoleExistsAsync(role);

                if (!result)
                {
                    var roleResult = await roleManager.CreateAsync(new Role(role));
                    result = roleResult.Succeeded;
                }

                return result;
            }

            static async Task<bool> CreateUserIfNotExist(UserManager<User> userManager, string userName, string password)
            {
                var user = await userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    var result = await userManager.CreateAsync(new User(userName), password);
                    return result.Succeeded;
                }

                return true;
            }

            static async Task<bool> AddToRoleIfNotExist(UserManager<User> userManager, string userName, string role)
            {
                var user = await userManager.FindByNameAsync(userName);

                if (!(await userManager.IsInRoleAsync(user, "Administrator")))
                {
                    var result = await userManager.AddToRoleAsync(user, role);
                    return result.Succeeded;
                }

                return true;
            }
        }

        public void Dispose()
        {
        }
    }
}
