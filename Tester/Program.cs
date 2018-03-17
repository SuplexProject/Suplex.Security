using System;
using System.Collections.Generic;

//using Newtonsoft.Json;

using Suplex.DataAccess;
using Suplex.DataAccess.Utilities;
using Suplex.Security.AclModel;
using Suplex.Security.Principal;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            SecureObject top = new SecureObject() { UniqueName = "top" };
            DiscretionaryAcl topdacl = new DiscretionaryAcl
            {
                new AccessControlEntry<FileSystemRight> { Allowed = true, Right = FileSystemRight.FullControl },
                new AccessControlEntry<FileSystemRight> { Allowed = false, Right = FileSystemRight.Execute | FileSystemRight.List, Inheritable = false },
                new AccessControlEntry<UIRight> { Right= UIRight.Operate | UIRight.Visible }
            };
            top.Security.Dacl = topdacl;

            List<User> users = new List<User>
            {
                new User{ Name = "x", IsBuiltIn = true, IsEnabled = true, IsLocal = true },
                new User{ Name = "y", IsBuiltIn = false, IsEnabled = false, IsLocal = false },
                new User{ Name = "z", IsBuiltIn = true, IsEnabled = false, IsLocal = true }
            };

            string foo = @"---
Users:
- UId: afcc9d02-0b81-463c-b4f8-782340f5b9fd
  Name: x
  IsLocal: true
  IsBuiltIn: true
  IsEnabled: true
- UId: 946ebca3-6a65-4422-aebd-aaa38107aaef
  Name: y
- UId: 120f901b-1957-4fd3-bee5-68aa2c7ab40d
  Name: z
  IsLocal: true
  IsBuiltIn: true
SecureObjects:
- Children: []
  UniqueName: top
  Security:
    DaclAllowInherit: true
    SaclAllowInherit: true
    Dacl:
    - UId: b19753f5-2ce6-4463-af39-0e7625b2acf6
      RightType: Suplex.Security.AclModel.FileSystemRight, Suplex.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
      Right: FullControl
      Allowed: True
      Inheritable: True
    - UId: aae5dd74-0610-49c8-8fa4-7e80cb2c78bd
      RightType: Suplex.Security.AclModel.FileSystemRight, Suplex.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
      Right: List, Execute
      Allowed: False
      Inheritable: False
    - UId: d6fc728e-8c57-4900-a57d-ae5328dc5877
      RightType: Suplex.Security.AclModel.UIRight, Suplex.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
      Right: Visible, Operate
      Allowed: True
      Inheritable: True
    Sacl: []
    Results: {}";



            FileStore store = new FileStore()
            {
                SecureObjects = new List<SecureObject>() { top },
                Users = users
            };
            string x = store.ToYaml();
            FileStore f = FileStore.FromYaml( x );

            f = FileStore.FromYaml( foo );

            User u0 = new User { Name = "g" };
            User u1 = new User { Name = "f", UId = u0.UId };

            f.Dal.UpsertUser( u0 );
            f.Dal.UpsertUser( u1 );
        }
    }
}


/*
 
        List<FooBase> foos = new List<FooBase>
            {
                new FooString { Goo = "x0", Yar = "y0", Tee = "t0" },
                new FooString { Goo = "x1", Yar = "y1", Tee = "t1" }
            };

    string xf = YamlHelpers.Serialize( foos );
    List<FooBase> fx = YamlHelpers.Deserialize<List<FooBase>>( xf );

    string output = JsonConvert.SerializeObject( foos );
    List<FooBase> input = JsonConvert.DeserializeObject<List<FooBase>>( output );


    public interface IFoo
    {
        string Goo { get; set; }
        string Yar { get; set; }
    }

    public class FooBase : IFoo
    {
        public virtual string Goo { get; set; }
        public virtual string Yar { get; set; }
    }

    public class Foo<T> : FooBase
    {
        public virtual T Tee { get; set; }

        public override string ToString()
        {
            return $"{Goo}/{Yar}/{Tee}";
        }
    }

    public class FooString : Foo<string>
    {
        //public override string Tee { get; set; }

        public override string ToString()
        {
            return $"{Goo}/{Yar}/{Tee}";
        }
    }


     */