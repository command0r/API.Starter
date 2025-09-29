using System.Collections.ObjectModel;

namespace API.Starter.Shared.Authorization;

public static class Action
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class Resource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
}

public static class Permissions
{
    private static readonly Permission[] AllArray = new Permission[]
    {
        new("View Dashboard", Action.View, Resource.Dashboard),
        new("View Hangfire", Action.View, Resource.Hangfire),
        new("View Users", Action.View, Resource.Users),
        new("Search Users", Action.Search, Resource.Users),
        new("Create Users", Action.Create, Resource.Users),
        new("Update Users", Action.Update, Resource.Users),
        new("Delete Users", Action.Delete, Resource.Users),
        new("Export Users", Action.Export, Resource.Users),
        new("View UserRoles", Action.View, Resource.UserRoles),
        new("Update UserRoles", Action.Update, Resource.UserRoles),
        new("View Roles", Action.View, Resource.Roles),
        new("Create Roles", Action.Create, Resource.Roles),
        new("Update Roles", Action.Update, Resource.Roles),
        new("Delete Roles", Action.Delete, Resource.Roles),
        new("View RoleClaims", Action.View, Resource.RoleClaims),
        new("Update RoleClaims", Action.Update, Resource.RoleClaims),
        new("View Products", Action.View, Resource.Products, IsBasic: true),
        new("Search Products", Action.Search, Resource.Products, IsBasic: true),
        new("Create Products", Action.Create, Resource.Products),
        new("Update Products", Action.Update, Resource.Products),
        new("Delete Products", Action.Delete, Resource.Products),
        new("Export Products", Action.Export, Resource.Products),
        new("View Brands", Action.View, Resource.Brands, IsBasic: true),
        new("Search Brands", Action.Search, Resource.Brands, IsBasic: true),
        new("Create Brands", Action.Create, Resource.Brands),
        new("Update Brands", Action.Update, Resource.Brands),
        new("Delete Brands", Action.Delete, Resource.Brands),
        new("Generate Brands", Action.Generate, Resource.Brands),
        new("Clean Brands", Action.Clean, Resource.Brands),
        new("View Tenants", Action.View, Resource.Tenants, IsRoot: true),
        new("Create Tenants", Action.Create, Resource.Tenants, IsRoot: true),
        new("Update Tenants", Action.Update, Resource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", Action.UpgradeSubscription, Resource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(AllArray);
    public static IReadOnlyList<Permission> Root { get; } = new ReadOnlyCollection<Permission>(AllArray.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<Permission> Admin { get; } = new ReadOnlyCollection<Permission>(AllArray.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<Permission> Basic { get; } = new ReadOnlyCollection<Permission>(AllArray.Where(p => p.IsBasic).ToArray());
}

public record Permission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}