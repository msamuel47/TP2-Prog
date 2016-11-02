/*
 * Écrire ici la documentation générale. Indiquez quel est le but de votre application.
 * 
 * Indiquez aussi qui est (sont) l' (les) auteur(s).
 * Auteur:
 * Co-auteur (si applicable):
 * */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TP2ETU.Properties;
using WMPLib;

namespace TP2ETU
{
    public partial class frmMemoryGameMain : Form
    {
        private int compteurDeMot;
        private int[] indexImagesEtMots;
        private int[] positionDesMots;
        private Random rnd = new Random();
        WindowsMediaPlayer mediaPlayer = new WindowsMediaPlayer();

        #region Propriétés /  variables partagées par toutes les méthodes.

        /// <summary>
        /// Tableau contenant tous les pictures box utilisées. Ce tableau est construit lors du chargement de la
        /// fenêtre (méthode Load)
        /// </summary>
        PictureBox[] tousLesPicturesBox = null;

        /// <summary>
        /// Tableau contenant toutes les images possibles pour le jeu.
        /// </summary>
        Bitmap[] toutesLesImagesAffichees = new Bitmap[]
        {
            Resources.imgHiddenCard, Resources.chat, Resources.chien, Resources.lapin, Resources.furet,
            Resources.grenouille, Resources.colibri, Resources.rat, Resources.souris, Resources.hamster,
            Resources.poisson
        };

        /// <summary>
        /// Tableau contenant les mots associés aux images du tableau toutesLesImagesAffichees.
        /// </summary>
        string[] tousLesTextesAssociesAuxImages = new string[]
        {
            "Cachée", "Chat", "Chien", "Lapin", "Furet", "Grenouille", "Colibri", "Rat", "Souris", "Hamster", "Poisson"
        };

        #endregion

        /// <summary>
        /// Appelée au chargement de l'application pour constituer le tableau des picturebox
        /// et initialiser correctement les valeurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMemoryGameMain_Load(object sender, EventArgs e)
        {
            // Création du tableau des picturebox
            tousLesPicturesBox = new PictureBox[]
            {
                pbImg0, pbImg1, pbImg2, pbImg3, pbImg4, pbImg5, pbImg6, pbImg7,
                pbImg8, pbImg9, pbImg10, pbImg11, pbImg12, pbImg13, pbImg14, pbImg15,
                pbImg16, pbImg17, pbImg18, pbImg19, pbImg20, pbImg21, pbImg22, pbImg23,
                pbImg24, pbImg25, pbImg26, pbImg27, pbImg28, pbImg29, pbImg30, pbImg31
            };

            // A COMPLETER AU BESOIN
        }

        #region Méthodes de gestion des événements et/ou appelées automatiquement

        public frmMemoryGameMain()
        {
            InitializeComponent();
        }
        #region fonction pour valider les mots

        /// <summary>
        /// La fonction sert à verifier les mots que les joueurs ont entré , elle extrait le texte
        /// qui était contenu dans la boîte de texte et calcul le 'score' que le joueur à obtenue.
        /// Contient également un système anti-triche
        /// </summary>
        void ValiderLeJeu()
        {
            if (mediaPlayer.playState == WMPPlayState.wmppsPlaying) //Fait arrêter la musique si elle
                                                                    //joue.
            {
                mediaPlayer.controls.stop();
            }
            string[] motsARechercher = ObtenirMotsRecherches();
            string[] motsAVerifier = ExtraireMotsEntresParJoueur();
            compteurDeMot = 0; // Sert à compter combien de mots on été trouvé
            for (int i = 0; i < motsAVerifier.Length; i++)
            {
                string motEnValidation = motsAVerifier[i];
                for (int j = 0; j < motsARechercher.Length; j++)
                {
                    string motValidateur = motsARechercher[j];
                    if (motEnValidation == motValidateur)
                    {
                        Debug.WriteLine("Mot trouvé :D");
                        compteurDeMot++;
                    }
                }
            }
        }

        string[] ObtenirMotsRecherches()
        {
            string[] listeDeMots = new string[indexImagesEtMots.Length];
            for (int i = 0; i < indexImagesEtMots.Length; i++)
            {
                listeDeMots[i] = tousLesTextesAssociesAuxImages[indexImagesEtMots[i]];
                Debug.WriteLine("Mot extrais : {0} trouvé dans l'indice {1} de l'indexImagesEtMots", listeDeMots[i],
                    i);
            }
            return listeDeMots;
        }

        string[] ExtraireMotsEntresParJoueur()
        {
            string texteAExtraire = TexteAEntrerEtValider.Text;
            string[] motsExtrais = texteAExtraire.Split(' ');
            motsExtrais = ExtraireSousElementsUniques(motsExtrais);
            return motsExtrais;
        }

        /// <summary>
        /// Sert à extraires seulement les mots rentrés une seule fois. Cela sert à empecher les triches 
        /// </summary>
        /// <param name="tableauAVerifier"></param>
        /// <returns>Le tableau avec seulement des mots uniques</returns>
        string[] ExtraireSousElementsUniques(string[] tableauAVerifier)
        {
            string[] motsExtrais = new string[tableauAVerifier.Length];
            motsExtrais[0] = tableauAVerifier[0];
            Debug.WriteLine("Le mot {0} a ete ajoute a l indice 0", motsExtrais[0]);
            string motAVerifier;
            int indiceAVerifier = 1;
            for (int i = 0; i < tableauAVerifier.Length; i++)
            {
                if (tableauAVerifier.Length <= 0)
                // Si le nombre de mot entr/ est de 1 , null besoin de besoin d<extraire les 
                {
                    // Les elements
                    break;
                }
                motAVerifier = tableauAVerifier[i];
                for (int j = 0; j < motsExtrais.Length; j++)
                {
                    if (motAVerifier == motsExtrais[j])
                    {
                        Debug.WriteLine("Tricherie détectée");
                        break;
                    }
                    if (j == motsExtrais.Length - 1)
                    {
                        motsExtrais[indiceAVerifier] = motAVerifier;
                        Debug.WriteLine("Le mot {0} a ete ajouter a l'indice {1} du tabeleau", motAVerifier,
                            indiceAVerifier);
                        indiceAVerifier++;
                    }
                }
            }
            return motsExtrais;
        }




        #endregion

        #region Fonction pour débuter le jeu

        /// <summary>
        /// Sert à masquer les mots qui sont affiché dans les pictures box pour empecher que le joueur
        /// voit les réponses
        /// </summary>
        void MasquerMots()
        {
            for (int i = 0; i < tousLesPicturesBox.Length; i++)
            {
                tousLesPicturesBox[i].BackgroundImage = toutesLesImagesAffichees[0];
            }
        }

        /// <summary>
        /// Sert à creer un index qui va distribué
        /// </summary>
        void ChoisirDesMots()
        {
            indexImagesEtMots = CreerTableauDeNombreAleatoireUnique(1, tousLesTextesAssociesAuxImages.Length - 1,
                (int) numericUpDownChoisirNbDeMot.Value);
        }

        /// <summary>
        /// Détermine les positions à mettre dans le tableau des picturesBox, 
        /// </summary>
        void ChoisirPositions()
        {
            positionDesMots = new int[indexImagesEtMots.Length];
            int testerPosition;
            bool motDejaPresent = false;
            for (int i = 0; i < indexImagesEtMots.Length; i++)
                {
                    testerPosition = rnd.Next(0, tousLesPicturesBox.Length);
                    do
                        {
                            for (int j = i; j >= 0; j--)
                                {
                                    if (testerPosition == positionDesMots[j])
                                        {
                                            motDejaPresent = true;
                                            testerPosition = rnd.Next(0, tousLesPicturesBox.Length);
                                        }
                                }
                        } while (motDejaPresent);
                    positionDesMots[i] = testerPosition;
                    
                }

        }

        void AfficherLesMots()
        {
            for (int i = 0; i < indexImagesEtMots.Length; i++)
                {
                    tousLesPicturesBox[positionDesMots[i]].BackgroundImage = toutesLesImagesAffichees[indexImagesEtMots[i]];
                }

        }

        #endregion

        #region fonction de triage des nombres

        /// <summary>
        /// Sert à créer un tableau de nombre différents. Il prend 3 paramètres. nbMinimum définie le chiffre minimum à être généré
        /// nbMaximum et celui du chiffre maximum. Longueur tableau sert à générer la longeur du tableau nécésaire.
        /// </summary>
        /// <param name="nbMinimum"></param>
        /// <param name="nbMaximum"></param>
        /// <param name="tailleTableau"></param>
        /// <returns></returns>
        int[] CreerTableauDeNombreAleatoireUnique(int nbMinimum, int nbMaximum, int tailleTableau)
        {
            int[] tableauARemplir = new int[tailleTableau];
            tableauARemplir[0] = rnd.Next(nbMinimum, nbMaximum);
            Debug.WriteLine("Le nombre {0} à été assigné à l'indice {1}", tableauARemplir[0], 0);
            int indiceAVerifier = nbMinimum - 1;
            for (int i = 0; i < tailleTableau; i++)
                {
                    if (tailleTableau <= 1)
                        {
                            Debug.WriteLine("Tableau plus petit ou égale à 1 ...");
                            break;
                        }
                    int indexAAssigner = rnd.Next(nbMinimum, nbMaximum + 1);
                    for (int j = 0; j <= indiceAVerifier; j++)
                        {
                            if (indexAAssigner == tableauARemplir[j])
                                {
                                    while (indexAAssigner == tableauARemplir[j])
                                        {
                                            Debug.WriteLine("Le nombre {0} est déja dans le tableau", indexAAssigner);
                                            indexAAssigner = rnd.Next(nbMinimum, nbMaximum + 1);
                                        }
                                }
                            else if (indiceAVerifier == j)
                                {
                                    tableauARemplir[j] = indexAAssigner;
                                    Debug.WriteLine("Le nombre {0} à été assigné à l'indice {1}", indexAAssigner, j);
                                    indiceAVerifier++;
                                    break;
                                }
                        }
                }
            return tableauARemplir;
        }

        #endregion

       

        private void pbImg0_Click(object sender, EventArgs e)
        {
        }

        private void recommencerPartieToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void qIEnHautDe85ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bienvenue au jeu de mémoire , le but consiste à trouver le plus de mots possible dans" +
                            " un jeu où les cartes sont affichées seulement durant un cour laps de temps",
                "Le jeu de mémoire", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void qiEnBasDe85ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult resulat = MessageBox.Show("Avez vous vraiment un QI inférieur à 85 ???", "Teste de QI",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resulat == DialogResult.Yes)
                {
                    MessageBox.Show("Vous êtes trop stupide pour jouer à ce jeu désolé :(", "Stupidié critique!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            else
                {
                    MessageBox.Show("Merci de ne pas avoir un QI trop faible vous pouvez reprendre le jeu",
                        "Lueure d'espoir...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void quitterLeJeuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult resultat = MessageBox.Show("Voulez-vous vraiment quitter ?", "Quitter le jeu",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultat == DialogResult.Yes)
                {
                    Application.Exit();
                }
            else if (resultat == DialogResult.No)
                {
                }
        }

        private void commencerLaMusiqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer.URL = "Art/background.mp3";
            mediaPlayer.controls.play();
        }

        private void ArreterLaMusiqueStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mediaPlayer.playState == WMPPlayState.wmppsPlaying)
                {
                    mediaPlayer.controls.stop();
                }
        }

        private void btnValiderLesMots_Click(object sender, EventArgs e)
        {
            ValiderLeJeu();
            TexteAEntrerEtValider.Text = "Vous avez trouvé " + compteurDeMot + " mots";
           AfficherLesMots();
        }


        private void btnDebuterPartie_Click(object sender, EventArgs e)
        {
        
            MasquerMots();
            timerCacherImage.Interval = (int)numericUpDownTimerModifierTemps.Value*1000;
            TexteAEntrerEtValider.Text = "";
            indexImagesEtMots = new int[tousLesTextesAssociesAuxImages.Length];
            compteurDeMot = 0;
            timerCacherImage.Stop();
            MasquerMots();
            ChoisirDesMots();
            ChoisirPositions();
            AfficherLesMots();
            timerCacherImage.Start();
        }

        private void timerCacherImage_Tick(object sender, EventArgs e)
        {

            MasquerMots();
            timerCacherImage.Stop();
        }
    }
}

#endregion