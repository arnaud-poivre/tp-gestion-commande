using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

class Program
{
    struct Produit
    {
        public int Id;
        public string Name;
        public decimal Prix;
    }

    struct Commande
    {
        public int Id;
        public string NomClient;
        public List<Produit> produits;
    }

    static List<Produit> produits = new List<Produit>();
    static List<Commande> commandes = new List<Commande>();

    static int compteurProduit = 1;
    static int compteurCommande = 1;
    static void Main()
    {
        bool continuer = true;
        while (continuer)
        {
            Console.WriteLine("\n1 - Ajouter un produit");
            Console.WriteLine("2 - Lister les produits");
            Console.WriteLine("3 - Nouvelle commande");
            Console.WriteLine("4 - Afficher commandes");
            Console.WriteLine("5 - Quitter");
            try
            {
                int choix = int.Parse(Console.ReadLine());
                switch (choix)
                {
                    case 1: AjouterProduit(); break;
                    case 2: AfficherProduits(); break;
                    case 3: NouvelleCommande(); break;
                    case 4: AfficherCommandes(); break;
                    case 5: continuer = false; break;
                    default: Console.WriteLine("Choix invalide."); break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Erreur de saisie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
    }

    static void AjouterProduit()
    {
        Console.WriteLine("Nom : ");
        string nom = Console.ReadLine();

        Console.WriteLine("Prix : ");
        decimal prix = decimal.Parse(Console.ReadLine());

        if (prix < 0)
        {
            throw new Exception("Le prix ne peut pas etre negatif");
        }

        Produit produit = new Produit();
        produit.Id = compteurProduit++;
        produit.Name = nom;
        produit.Prix = prix;

        produits.Add(produit);
        Console.WriteLine($"Produit ajouté avec Id : {produit.Id}");

    }

    static void AfficherProduits()
    {
        if (produits.Count == 0)
        {
            Console.WriteLine("Aucun produit à afficher");
            return;
        }

        foreach (var produit in produits)
        {
            Console.WriteLine($"Liste des produits :");
            Console.WriteLine($"{produit.Id} - {produit.Name} - {produit.Prix}");
        }

    }

    static void NouvelleCommande()
    {
        if (produits.Count == 0)
            throw new InvalidOperationException("Aucun produit disponible.");

        Console.Write("Nom du client : ");
        string nomClient = Console.ReadLine();

        List<Produit> produitsCommande = new List<Produit>();
        bool continuer = true;

        while (continuer)
        {
            Console.WriteLine("\nProduits disponibles :");
            foreach (var p in produits)
            {
                Console.WriteLine($"{p.Id} - {p.Name} - {p.Prix} euros");
            }
            Console.Write("\nId du produit à ajouter (0 pour terminer) : ");
            int idProduit = int.Parse(Console.ReadLine());
            if (idProduit == 0)
            {
                continuer = false;
            }
            else
            {
                Produit? produitTrouve = null;
                foreach (var p in produits)
                {
                    if (p.Id == idProduit)
                    {
                        produitTrouve = p;
                        break;
                    }
                }
                if (produitTrouve == null)
                    throw new InvalidOperationException("Produit introuvable.");
                produitsCommande.Add(produitTrouve.Value);
                Console.WriteLine("Produit ajouté.");
            }
        }
        if (produitsCommande.Count == 0)
            throw new InvalidOperationException("Une commande doit contenir au moins un produit.");

        Commande commande = new Commande
        {
            Id = compteurCommande++,
            NomClient = nomClient,
            produits = produitsCommande
        };

        commandes.Add(commande);
        Console.WriteLine($"\nCommande créée avec succès ! Numéro : {commande.Id}");
    }

    static void AfficherCommandes()
    {
        if (commandes.Count == 0)
        {
            Console.WriteLine("Aucune commande.");
            return;
        }
        foreach (var c in commandes)
        {
            Console.WriteLine($"\nCommande {c.Id} - {c.NomClient}");
            decimal total = 0;
            foreach (var p in c.produits)
            {
                Console.WriteLine($" {p.Name} - {p.Prix} euros");
                total += p.Prix;
            }
            Console.WriteLine($"Total : {total} euros");
        }
    }

}
