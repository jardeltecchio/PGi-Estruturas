using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public partial class TPavimento
    {
        void MalhaTesteBasica()
        {
            nos[++max_sNo] = new TNoGrelha(0, 0, 0, 0, 3, max_sNo + 1, null, null, false, true);
            nos[++max_sNo] = new TNoGrelha(100, 0, 0, 0, 0, max_sNo + 1, null, null, false, true);
            nos[++max_sNo] = new TNoGrelha(200, 0, 0, 0, 0, max_sNo + 1, null, null, false, true);
            nos[++max_sNo] = new TNoGrelha(300, 0, 0, 0, 0, max_sNo + 1, null, null, false, true);
            nos[++max_sNo] = new TNoGrelha(400, 0, 0, 0, 3, max_sNo + 1, null, null, false, true);
            // nos[++max_sNo] = new TNoGrelha(500, 0, 0, 0, 3, max_sNo + 1, null, null, false, true);

            barras[++max_sBarra] = (new TBarraGrelha(nos[1], nos[2], null, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            barras[++max_sBarra] = (new TBarraGrelha(nos[2], nos[3], null, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            barras[++max_sBarra] = (new TBarraGrelha(nos[3], nos[4], null,  0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            barras[++max_sBarra] = (new TBarraGrelha(nos[4], nos[5], null,  0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            //   barras[++max_sBarra] = (new TBarraGrelha(nos[5], nos[6], null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
            //                                             false, max_sBarra, false));
            RefazerGrelha = false;
        }

        void Soriano()
        {
            nos[++max_sNo] = new TNoGrelha(0, 0, 0, 0, 2, max_sNo + 1, null, null, false, true, new double[4] {0,0,0,0});
            nos[++max_sNo] = new TNoGrelha(0, -4, 0, 0, 0, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, -5 });
            nos[++max_sNo] = new TNoGrelha(6, -4, 0, 0, 2, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, 0 });
            // nos[++max_sNo] = new TNoGrelha(500, 0, 0, 0, 3, max_sNo + 1, null, null, false, true);

            barras[++max_sBarra] = (new TBarraGrelha(nos[1], nos[2], null, -10, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            barras[++max_sBarra] = (new TBarraGrelha(nos[2], nos[3], null, -5, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
   
            for (int uu = 1; uu <= 4; uu++)
            {
                barras[uu].E = 50; barras[uu].I = 1; barras[uu].G = 40; barras[uu].I = 1;
            }

             //   barras[++max_sBarra] = (new TBarraGrelha(nos[5], nos[6], null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
            //                                             false, max_sBarra, false));
            RefazerGrelha = false;
        }

        void Igor()
        {
            nos[++max_sNo] = new TNoGrelha(0, 0, 0, 0, 1, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, 0 });
            nos[++max_sNo] = new TNoGrelha(1, 0, 0, 0, 1, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, -10, 0 });
            nos[++max_sNo] = new TNoGrelha(1, 1, 0, 0, 1, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, 0 });
            // nos[++max_sNo] = new TNoGrelha(500, 0, 0, 0, 3, max_sNo + 1, null, null, false, true);

            barras[++max_sBarra] = (new TBarraGrelha(nos[1], nos[2], null,  0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            barras[++max_sBarra] = (new TBarraGrelha(nos[2], nos[3], null,  0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            //   barras[++max_sBarra] = (new TBarraGrelha(nos[5], nos[6], null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
            //                                             false, max_sBarra, false));
            RefazerGrelha = false;
        }

        void Sussekind_Pg40()
        {
            nos[++max_sNo] = new TNoGrelha(0, 0, 0, 0, 2, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, 0 });
            nos[++max_sNo] = new TNoGrelha(0, -500, 0, 0, 1, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, 0 });
            nos[++max_sNo] = new TNoGrelha(500, -500, 0, 0, 0, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, -40 });
            nos[++max_sNo] = new TNoGrelha(1000, -500, 0, 0, 1, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, 0 });
            nos[++max_sNo] = new TNoGrelha(1000, 0, 0, 0, 2, max_sNo + 1, null, null, false, true, new double[4] { 0, 0, 0, 0 });
            // nos[++max_sNo] = new TNoGrelha(500, 0, 0, 0, 3, max_sNo + 1, null, null, false, true);

            barras[++max_sBarra] = (new TBarraGrelha(nos[1], nos[2], null,  -12f, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            barras[++max_sBarra] = (new TBarraGrelha(nos[2], nos[3], null,  0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            barras[++max_sBarra] = (new TBarraGrelha(nos[3], nos[4], null, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            barras[++max_sBarra] = (new TBarraGrelha(nos[4], nos[5], null,  0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, max_sBarra, false));
            //   barras[++max_sBarra] = (new TBarraGrelha(nos[5], nos[6], null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
            //                                             false, max_sBarra, false));
            for (int uu = 1; uu <= 4; uu++)
            {
                barras[uu].E = 50; barras[uu].I = 1; barras[uu].G = 40; barras[uu].I = 1;
            }
            
            RefazerGrelha = false;
        }

        /*
         sussekind - análise estrutural volume III
         soriano - análise de estruturas       
        */

        private bool GerarGrelha_Teste(string nomeGrelha)
        {
            vigas.Add(null);
            pilares.Add(null);

            Inicializa();

            if (nomeGrelha == "basico")
              MalhaTesteBasica();
         
            if (nomeGrelha == "soriano")
              Soriano();

            if (nomeGrelha == "igor")
                Igor();

            if (nomeGrelha == "sussekind_pg40")
                Sussekind_Pg40(); 
            
            Finaliza();

            return true;
        }
    }
}
