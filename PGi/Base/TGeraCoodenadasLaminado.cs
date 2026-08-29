using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public class TGeraCoodenadasRetangulo
    {
        public CoordenadaD[] coordenadas;
        TPropriedades_Perfil_Retangulo Dados;
        public TGeraCoodenadasRetangulo() { }

        public TGeraCoodenadasRetangulo(TPropriedades_Perfil_Retangulo Dados_Perfil)
        {
            Dados = Dados_Perfil;

            Gera_Solido_Ret();
        }
        public List<CoordenadaD> coords;
        void Gera_Solido_Ret()
        {
            try
            {
                coords = new List<CoordenadaD>();
                coords.Add(new CoordenadaD(0, 0, false));
                coords.Add(new CoordenadaD(Dados.B, 0, false));
                coords.Add(new CoordenadaD(Dados.B, Dados.H, false));
                coords.Add(new CoordenadaD(0, Dados.H, false));
                coords.Add(new CoordenadaD(0, 0, false));

                coordenadas = new CoordenadaD[coords.Count];
                for (int i = 0; i < coords.Count; i++)
                {
                    coordenadas[i] = coords[i];
                    coordenadas[i].X /= 100;
                    coordenadas[i].Y /= 100;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao gerar desenho do perfil sólido: " + e.Message);
            }
        }

    }
    public class TGeraCoodenadasLaminado
    {
        public CoordenadaD[] coordenadas;
        TPropriedades_Perfil_W_Gerdau Dados;
        public TGeraCoodenadasLaminado() { }

        public TGeraCoodenadasLaminado(TPropriedades_Perfil_W_Gerdau Dados_Perfil, int qtd_segmentos_raio)
        {
            Dados = Dados_Perfil;
            
            Gera_I_W_Laminado(qtd_segmentos_raio);
        }
        public List<CoordenadaD> coords;
        public void CriaRaio(ref List<vec3> pts, double x_ini, double y_ini, double r, double start_angle, double arc_angle, int segmentos)
        {
            pts = new List<vec3>();
            double theta = arc_angle / (double)(segmentos - 1);//theta is now calculated from the arc angle instead, the - 1 bit comes from the fact that the arc is open
            double tangetial_factor = System.Convert.ToSingle(Math.Tan(theta * Const.PIDiv180));
            double radial_factor = System.Convert.ToSingle(Math.Cos(theta * Const.PIDiv180));

            double x = r * System.Convert.ToSingle(Math.Cos(start_angle * Const.PIDiv180));//we now start at the start angle
            double y = r * System.Convert.ToSingle(Math.Sin(start_angle * Const.PIDiv180));

            for (int ii = 0; ii < segmentos; ii++)
            {
                float pixx = System.Convert.ToSingle(x + x_ini);
                float pixy = System.Convert.ToSingle(y + y_ini);

                pts.Add(new vec3(pixx, pixy, 0));
                double tx = -y;
                double ty = x;

                x += tx * tangetial_factor;
                y += ty * tangetial_factor;

                x *= radial_factor;
                y *= radial_factor;
            }
        }
        List<vec3> pontos_raio;
        void Gera_I_W_Laminado(int qtd_segmentos_raio)
        {
            try
            {
                int segmentos_raio = qtd_segmentos_raio;
                pontos_raio = new List<vec3>();
                double raio = ((Dados.D / 2) - (Dados.dlinha / 2) - Dados.TF);

                coords = new List<CoordenadaD>();
                coords.Add(new CoordenadaD(0, 0, false));
                coords.Add(new CoordenadaD(Dados.BF, 0, false));
                coords.Add(new CoordenadaD(Dados.BF, Dados.TF, false));

                double x1 = (Dados.BF) - ((Dados.BF) / 2) + ((Dados.TW) / 2) + raio;
                double y1 = Dados.TF + raio;
                CriaRaio(ref pontos_raio, x1, y1, raio, 270, -90, segmentos_raio);

                for (int i = 0; i < pontos_raio.Count; i++)               
                    coords.Add(new CoordenadaD(pontos_raio[i].x, pontos_raio[i].y, i==0?true:true));

                x1 = (Dados.BF) - ((Dados.BF) / 2) + ((Dados.TW) / 2) + raio;
                y1 = coords[coords.Count - 1].Y + Dados.dlinha;
                CriaRaio(ref pontos_raio, x1, y1, raio, 180, -90, segmentos_raio);
                for (int i = 0; i < pontos_raio.Count; i++)
                    coords.Add(new CoordenadaD(pontos_raio[i].x, pontos_raio[i].y, i == 0 ? true : true));

                /////////// tirar--teste
               // coords.Add(new CoordenadaD(Dados.BF, Dados.D - 20, false));

                coords.Add(new CoordenadaD(Dados.BF, coords[coords.Count - 1].Y, false));
                coords.Add(new CoordenadaD(Dados.BF, Dados.D, false));
                coords.Add(new CoordenadaD(0, Dados.D, false));
                coords.Add(new CoordenadaD(0, Dados.D - Dados.TF, false));

                x1 = (Dados.BF) - ((Dados.BF) / 2) - ((Dados.TW) / 2) - raio;
                y1 = Dados.D - Dados.TF - raio;
                CriaRaio(ref pontos_raio, x1, y1, raio, 90, -90, segmentos_raio);
                for (int i = 0; i < pontos_raio.Count; i++)
                    coords.Add(new CoordenadaD(pontos_raio[i].x, pontos_raio[i].y, i == 0 ? true : true));

                x1 = (Dados.BF) - ((Dados.BF) / 2) - ((Dados.TW) / 2) - raio;
                y1 = Dados.TF + raio;
                CriaRaio(ref pontos_raio, x1, y1, raio, 0, -90, segmentos_raio);
                for (int i = 0; i < pontos_raio.Count; i++)
                    coords.Add(new CoordenadaD(pontos_raio[i].x, pontos_raio[i].y, i == 0 ? true : true));

                coords.Add(new CoordenadaD(0, Dados.TF, false));
               
                /////////// tirar--teste
               coords.Add(new CoordenadaD(-29, Dados.TF-27, false));
                
                
                coords.Add(new CoordenadaD(0, 0, false));

                coordenadas = new CoordenadaD[coords.Count];
                for (int i = 0; i < coords.Count; i++)
                {
                    coordenadas[i] = coords[i];
                    coordenadas[i].X /= 1000;
                    coordenadas[i].Y /= 1000;
                }
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao gerar desenho do perfil laminado: " + e.Message);
            }

        }
    }
}
