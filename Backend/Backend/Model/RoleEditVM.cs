
using Microsoft.AspNetCore.Identity;
using Models;

namespace JosephProfile.Model;

public class RoleEditVM
{

	public IdentityRole  Role { get; set; }
	public IEnumerable<UserVM> Memeber { get; set; }
    public IEnumerable<UserVM> NonMemeber { get; set; }
    public string RoleName { get; set; }
    public string[] AddIds { get; set; }
    public string[] DeleteIds { get; set; }

}

