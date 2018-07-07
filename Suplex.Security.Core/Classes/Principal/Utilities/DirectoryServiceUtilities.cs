using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Suplex.Security.Principal
{
    public class DirectoryServicesHelper
    {
        public DirectoryServicesHelper()
        {
        }

        public DirectoryServicesHelper(string ldapRoot, List<string> externalGroups = null, string authUserName = null, string authPassword = null)
        {
            LdapRoot = ldapRoot;
            ExternalGroups = externalGroups;

            AuthUser = authUserName;
            AuthPassword = authPassword;
        }

        public string LdapRoot { get; set; }
        public bool HasLdapRoot { get { return !string.IsNullOrEmpty( LdapRoot ); } }

        public List<string> ExternalGroups { get; set; }
        public bool HasExternalGroups { get { return ExternalGroups?.Count > 0; } }

        public string AuthUser { get; set; }
        public bool HasAuthUser { get { return !string.IsNullOrEmpty( AuthUser ); } }
        public string AuthPassword { get; set; }
        public bool HasAuthPassword { get { return !string.IsNullOrEmpty( AuthPassword ); } }

        public List<string> GroupMembership { get; private set; }


        public List<string> GetGroupMembership(string userName)
        {
            GroupMembership = HasExternalGroups ? ExternalGroups : new List<string>();
            GetGroupMembershipInternal( userName );
            return GroupMembership;
        }

        public static List<string> GetGroupMembership(string userName, string ldapRoot, List<string> externalGroups = null, string authUserName = null, string authPassword = null)
        {
            return new DirectoryServicesHelper( ldapRoot, externalGroups, authUserName, authPassword ).GetGroupMembership( userName );
        }

        void GetGroupMembershipInternal(string userName)
        {
            if( HasLdapRoot )
            {
                string[] user = userName.Split( '\\' );
                string name = user[0];
                if( user.Length > 1 )
                    name = user[1];

                DirectoryEntry root = new DirectoryEntry( LdapRoot );
                if( !string.IsNullOrEmpty( AuthUser ) )
                {
                    root.Username = AuthUser;
                    root.Password = AuthPassword;
                }
                DirectorySearcher groups = new DirectorySearcher( root );
                groups.Filter = "sAMAccountName=" + name;
                groups.PropertiesToLoad.Add( "memberOf" );

                SearchResult sr = groups.FindOne();
                if( sr != null )
                {
                    for( int i = 0; i <= sr.Properties["memberOf"].Count - 1; i++ )
                    {
                        string group = sr.Properties["memberOf"][i].ToString();
                        GroupMembership.Add( group.Split( ',' )[0].Replace( "CN=", "" ) );
                    }
                }
            }
        }
    }
}