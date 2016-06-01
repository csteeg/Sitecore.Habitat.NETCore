using System;

namespace Habitat.Core.Repositories.Navigation
{
  public struct Templates
  {
    public struct NavigationRoot
    {
      public static readonly Guid Id = new Guid("{F9F4FC05-98D0-4C62-860F-F08AE7F0EE25}");
    }

    public struct Navigable
    {
      public static readonly Guid Id = new Guid("{A1CBA309-D22B-46D5-80F8-2972C185363F}");

    }

    public struct Link
    {
      public static readonly Guid Id = new Guid("{A16B74E9-01B8-439C-B44E-42B3FB2EE14B}");

    }

    public struct LinkMenuItem
    {
      public static readonly Guid Id = new Guid("{18BAF6B0-E0D6-4CCE-9184-A4849343E7E4}");

    }
  }
}