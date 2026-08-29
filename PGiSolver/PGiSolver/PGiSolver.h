// PGiSolver.h
#pragma once
#include "list";
#include "string";
#include "solvers.h";

using namespace System;
using namespace System::IO;
using namespace std;
using std::list;
using std::string;
using namespace System::Collections::Generic;
using namespace alglib;

namespace PGiSolver 
{

	public ref class Solver
	{
		public:
		Solver(array<array<double>^>^ m_rigidez)
		{

		}
		static double Foo()
		{
			return 52;
		}
		
		static int ResolveAlgLib(array<array<double>^>^% Banda, array<double>^% ac, array<double>^% df, array<double>^% Sff, int nLinhas, int nColunas, int tamBanda)
		{
		//	array<array<double>^>^ Banda;
			//FileStream^ MyFileStream = nullptr;
			//MyFileStream = gcnew FileStream("C:\\PGi\\1.dat", FileMode::Create, FileAccess::ReadWrite);
			try
			{

				int banda = tamBanda;
				alglib::sparsematrix s;

				//MyFileStream = gcnew FileStream("C:\\PGi\\2.dat", FileMode::Create, FileAccess::ReadWrite);
				alglib::sparsecreatesksband(nLinhas, nLinhas, banda, s);
				//MyFileStream = gcnew FileStream("C:\\PGi\\3.dat", FileMode::Create, FileAccess::ReadWrite);

				/*Banda = gcnew array<array<double>^>(nLinhas);

				for(int i = 0; i < nLinhas; i++)
				Banda[i] = gcnew array<double>(nColunas);

				banda = nColunas-1;*/
				//MyFileStream = gcnew FileStream("C:\\PGi\\ncolunas_"+nColunas+".dat", FileMode::Create, FileAccess::ReadWrite);
				//MyFileStream = gcnew FileStream("C:\\PGi\\ncolunas_"+nLinhas+".dat", FileMode::Create, FileAccess::ReadWrite);
				//MyFileStream = gcnew FileStream("C:\\PGi\\banda_"+banda+".dat", FileMode::Create, FileAccess::ReadWrite);
				double coef;
				int colMatriz, colBanda, i, j;
				for(i = 0; i<nLinhas; i++)
				{
					colBanda = 0;
					colMatriz = i;
					for(j = 0; j<nColunas; j++)
					{
						if(colBanda<nLinhas)
						{
							coef = Banda[i][colBanda];

							if(coef!=0)
								alglib::sparseset(s, i, colMatriz, coef);

							colMatriz++;
							colBanda++;
						}
						else
							break;
					}
				}

				//MyFileStream = gcnew FileStream("C:\\PGi\\4.dat", FileMode::Create, FileAccess::ReadWrite);
				//Sff2 = null;
				//Sff_ = null;
				Banda = nullptr;
				//System.GC.Collect();*/

				alglib::real_1d_array b;// = gcnew array<double>(nLinhas);
				b.setlength(nLinhas);
				for(int i = 1; i<=nLinhas; i++)
					b[i-1] = ac[i];

				alglib::sparsesolverreport rep;
				alglib::real_1d_array x;
				bool isuppertriangle = true;
				alglib::sparsesolvesks(s, nLinhas, isuppertriangle, b, rep, x);

				for(int i = 0; i<nLinhas; i++)
					df[i+1] = x[i];

				//alglib::deallocateimmediately(ref s);
				//x = nullptr;
				//rep = nullptr;
				
				//ac = nullptr;
				
				System::GC::Collect();
			}
			catch(OutOfMemoryException^)
			{
				//      MessageBox.Show("Falta de memória!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return -1;
			}
			return 0;
		}

		static bool FatoraMatrizBanda(array<double>^% Sff, int nLinhas, int nColunas)
		{
			int j, j1, j2, i, i1, k;
			double sum, temp;
			try
			{
				if(Sff[1*nColunas+1]<=0) return false;

				for(j = 2; j<=nLinhas; j++)
				{
					j1 = j-1;
					j2 = j-nColunas+1;

					if(j2<1)
						j2 = 1;

					if(j1!=1)
					{
						//     Application.DoEvents();

						for(i = 2; i<=j1; i++)
						{
							i1 = i-1;

							if(i1>=j2)
							{
								sum = Sff[i * nColunas+j-i+1];

								for(k = j2; k<=i1; k++)
									sum = sum-Sff[k * nColunas+i-k+1]*Sff[k * nColunas+j-k+1];

								Sff[i * nColunas+j-i+1] = sum;
							}
						}
					};

					sum = Sff[j * nColunas+1];
					for(k = j2; k<=j1; k++)
					{
						temp = Sff[k * nColunas+j-k+1]/Sff[k * nColunas+1];
						sum = sum-temp * Sff[k * nColunas+j-k+1];
						Sff[k * nColunas+j-k+1] = temp;
					}

					if(sum<=0)
					{
						return false;
					}
					Sff[j * nColunas+1] = sum;
				};
			}
		
			catch(OutOfMemoryException^)
			{
				//      MessageBox.Show("Falta de memória!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			catch(IndexOutOfRangeException ^)
			{
		//		MessageBox.Show("Pavimento:  "+Modelo.Descricao+"  -  Erro na solução do sistema! Verifique as condições de suporte.", "Matriz de rigidez", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);

				return false;
			}
		//
			return true;
		}


		static int ResolveMatrizBanda(array<double>^% Sff, array<double>^% ac, array<double>^% df, int nLinhas, int nColunas)
		{
			int i, j, k, i1, k1, k2;
			double sum;
			//guardar a coluna 1 de sff para usar no segundo loop
			array<double>^ sff1 = gcnew array<double>(nLinhas+1);

			for(i = 1; i<=nLinhas; i++)
			{
				j = i-nColunas+1;

				if(i<=nColunas)
					j = 1;

				sum = ac[i];
				k1 = i-1;

				for(k = j; k<=k1; k++)
					sum = sum-Sff[k * nColunas+i-k+1]*df[k];

				df[i] = sum;

				sff1[i] = Sff[i * nColunas+1];
			}

			for(i = 1; i<=nLinhas; i++)
				df[i] = df[i]/sff1[i];

			for(i1 = 1; i1<=nLinhas; i1++)
			{
				i = nLinhas-i1+1;
				j = i+nColunas-1;

				if(j > nLinhas)
					j = nLinhas;

				sum = df[i];

				k2 = i+1;

				for(k = k2; k<=j; k++)
					sum = sum-Sff[i * nColunas+k-i+1]*df[k];

				df[i] = sum;
			}

			return 0;
		}
	};
}
