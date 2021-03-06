﻿using System;
using System.Linq;
using GThor.Views.Veiculos;
using GThorFrameworkBiblioteca.Ferramentas.HelpersHidratacaoValores;
using GThorFrameworkDominio.Dominios.Veiculos;
using GThorFrameworkWpf.Models.DataGrid;
using GThorNegocio.Contratos;
using GThorNegocio.Criadores;

namespace GThor.Models.Veiculos
{
    public class GridVeiculoModel : DataGridPadraoModel<Veiculo>
    {
        private readonly INegocioVeiculo _negocioVeiculo;

        public GridVeiculoModel(INegocioVeiculo negocioVeiculo)
        {
            _negocioVeiculo = negocioVeiculo;
        }

        public override void BuscarRegistros()
        {
            Cache = _negocioVeiculo.Lista();
            PreencherLista(Cache);
        }

        public override void AplicaPesquisa(string pesquisarTexto)
        {
            if (pesquisarTexto.IsNotNullOrEmpty())
            {
                var listaFiltrada = Cache.Where(
                    v => v.Id.ToString() == pesquisarTexto
                    || v.Descricao.Contains(pesquisarTexto)
                    || v.CodigoInterno == pesquisarTexto
                    || v.Placa.Contains(pesquisarTexto)
                    || v.Renavam == pesquisarTexto).ToList();

                PreencherLista(listaFiltrada);
                return;
            }

            PreencherLista(Cache.ToList());
        }

        public override void CriarColunas()
        {
            Veiculo veiculo = null;
            AdicionarDataGridColumn(() => veiculo.Id, 40);
            AdicionarDataGridColumn(() => veiculo.Descricao, 400);
            AdicionarDataGridColumn(() => veiculo.CodigoInterno, 100);
            AdicionarDataGridColumn(() => veiculo.Placa, 100);
            AdicionarDataGridColumn(() => veiculo.Renavam, 200);
            AdicionarDataGridColumn(() => veiculo.TaraEmKg, 80);
            AdicionarDataGridColumn(() => veiculo.CapacidadeEmKg, 80);
            AdicionarDataGridColumn(() => veiculo.CapacidadeEmM3, 80);
            AdicionarDataGridColumn(() => veiculo.TipoRodado, 150);
            AdicionarDataGridColumn(() => veiculo.TipoCarroceria, 150);
        }

        public override void NovoRegistroAction(object obj)
        {
            var model = new VeiculoFormModel(NegocioCriador.CriaNegocioVeiculo()) {Veiculo = new Veiculo()};

            model.AtualizarListaHandler += AtualizarLista;

            new VeiculoForm(model).ShowDialog();
        }

        private void AtualizarLista(object sender, EventArgs e)
        {
            IniciaPesquisa(PesquisarTexto);
        }

        public override void DuploClickDataGrid()
        {
            var veiculo = _negocioVeiculo.CarregarPorId(EntidadeSelecionada.Id);

            var model = new VeiculoFormModel(NegocioCriador.CriaNegocioVeiculo()) { Veiculo =  veiculo};

            model.AtualizarListaHandler += AtualizarLista;

            new VeiculoForm(model).ShowDialog();
        }

        protected override void DeletarRegistroSelecionado()
        {
            _negocioVeiculo.Deletar(EntidadeSelecionada);
        }
    }
}