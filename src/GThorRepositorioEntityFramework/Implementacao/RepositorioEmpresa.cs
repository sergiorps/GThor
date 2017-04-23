﻿using System.Collections.Generic;
using System.Linq;
using GThorFrameworkDominio.Dominios.Empresas;
using GThorRepositorio.Contratos;
using GThorRepositorioEntityFramework.Implementacao.Base;

namespace GThorRepositorioEntityFramework.Implementacao
{
    internal class RepositorioEmpresa : RepositorioBase, IRepositorioEmpresa
    {
        public Empresa CarregarPorId(int id)
        {
            return GThorContexto.Empresas.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Empresa> Lista()
        {
            return GThorContexto.Empresas.ToList();
        }

        public void SalvarOuAtualizar(Empresa entity)
        {
            if (entity.Id == 0)
            {
                GThorContexto.Empresas.Add(entity);
                return;
            }

            GThorContexto.Empresas.Update(entity);
        }

        public void Deletar(Empresa entity)
        {
            GThorContexto.Empresas.Remove(entity);
        }
    }
}