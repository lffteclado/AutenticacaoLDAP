using System;
using System.Net;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.Security.Permissions;

namespace LdapConsole
{
    [DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
    public class Program
    {
        private static string LdapServidor
        {
            get { return "192.168.100.5"; }
        }

        private static int LdapPorta
        {
            get { return 389; }
        }

        private static string LdapUsuario
        {
            get { return "zimbra"; }
        }

        private static string LdapSenha
        {
            get { return "DrpmsNtqWu"; }
        }

        public static void Main(string[] args)
        {
            Autentica5("uid=zimbra,cn=admins,cn=zimbra", "DrpmsNtqWu");

            //try
            //{
            //    // Create the new LDAP connection
            //    LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier("192.168.100.5", 389);
            //    LdapConnection ldapConnection = new LdapConnection(ldi);
            //    Console.WriteLine("LdapConnection is created successfully.");
            //    ldapConnection.AuthType = AuthType.Basic;
            //    ldapConnection.SessionOptions.ProtocolVersion = 3;
            //    NetworkCredential nc = new NetworkCredential("uid=luisfelipe,ou=people,dc=grupovdl,dc=com,dc=br", ""); //password
            //    //NetworkCredential nc = new NetworkCredential("uid=zimbra,cn=admins,cn=zimbra","DrpmsNtqWu"); //password
            //    ldapConnection.Bind(nc);
            //    Console.WriteLine("LdapConnection authentication success");           

            //    ldapConnection.Dispose();
            //}
            //catch (LdapException e)
            //{
            //    Console.WriteLine("\r\nUnable to login:\r\n\t" + e.Message);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("\r\nUnexpected exception occured:\r\n\t" + e.GetType() + ":" + e.Message);
            //}

            //Console.Read();
        }

        

        static bool Autentica (string userName, string senha)
        {
            bool autenticado = false;

            string retorno = string.Empty;

            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://192.168.100.5:389", userName, senha);
                retorno = entry.Name;
                object nativeObject = entry.NativeObject;
                autenticado = true;
            }
            catch(DirectoryServicesCOMException e)
            {
                Console.WriteLine(e.Message);
            }

            return autenticado;
        }


        static void Autentica2(string userName, string senha)
        {
            try
            { 

                DirectoryEntry ldap = new DirectoryEntry("LDAP://192.168.100.5:389/dc=grupovdl,dc=com,dc=br", userName, senha);

                DirectorySearcher busca = new DirectorySearcher(ldap);
                busca.Filter = "(cn=*)";
                SearchResultCollection resultado = busca.FindAll();

                foreach (SearchResult result in resultado)
                {
                    string uid = Convert.ToString(result.Properties["displayname"][0]);
                    //Lista.Items.Add(uid);

                    Console.Write(uid);
                }

            }
            catch (DirectoryServicesCOMException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        static void Autentica3(string userName, string senha)
        {
            try
            {

                LdapConnection ldapconn = new LdapConnection("LDAP://zimbra.grupovdl.com.br:389");
                ldapconn.AuthType = AuthType.Basic;
                ldapconn.Credential = new NetworkCredential(userName, senha);
                ldapconn.Bind();

            }
            catch (DirectoryServicesCOMException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        static void Autentica4(string userName, string senha)
        {
            try
            {

                DirectoryEntry de = new DirectoryEntry();
                de.Path = "LDAP://192.168.100.5:389";
                de.AuthenticationType = AuthenticationTypes.Secure;

                DirectorySearcher deSearch = new DirectorySearcher();

                deSearch.SearchRoot = de;
                deSearch.Filter = "(&(objectClass=user) (cn=" + userName + "))";

                SearchResult result = deSearch.FindOne();

                if (result != null)
                {
                    DirectoryEntry deUsers = new DirectoryEntry(result.Path);

                    string senhaUsers = deUsers.Name;

                    deUsers.Close();
                }

            }
            catch (DirectoryServicesCOMException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        static void Autentica5(string userName, string senha)
        {

            DirectoryEntry ldap = new DirectoryEntry("LDAP://zimbra.grupovdl.com.br:389/dc=grupovdl,dc=com,dc=br", userName, senha);

            ldap.AuthenticationType = AuthenticationTypes.FastBind;

            DirectorySearcher busca = new DirectorySearcher(ldap);
            busca.Filter = "(uid=*)";
            SearchResultCollection resultado = busca.FindAll();

            foreach (SearchResult result in resultado)
            {
                string uid = Convert.ToString(result.Path);

                Console.WriteLine(uid);
                //Lista.Items.Add(uid);
            }

            Console.Read();

        }




}
}
