﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using GThorFrameworkDominio.Dominios.Cidades;
using GThorFrameworkDominio.Dominios.EstadosUf;
using GThorNegocio.Criadores;
using JetBrains.Annotations;

namespace GThorFrameworkComponentes.ComboBox.EstadosUFs
{
    public partial class ComboBoxCidade : INotifyPropertyChanged
    {
        private static IEnumerable<Cidade> _cacheCidades;

        private static readonly RoutedEvent PickItemCidadeEvent =
            EventManager.RegisterRoutedEvent("PickItemCidade", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ComboBoxUf));

        public event RoutedEventHandler PickItemCidade
        {
            add => AddHandler(PickItemCidadeEvent, value);
            remove => RemoveHandler(PickItemCidadeEvent, value);
        }

        public static readonly DependencyProperty CLabelProperty =
            DependencyProperty.Register("CLabel", typeof(string), typeof(ComboBoxCidade), new PropertyMetadata(string.Empty, CLabelCallback));

        private static void CLabelCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBoxUf = d as ComboBoxCidade;

            if (comboBoxUf == null) return;

            comboBoxUf.CLabelNome.Text = e.NewValue.ToString();
        }

        public string CLabel
        {
            get => (string)GetValue(CLabelProperty);
            set => SetValue(CLabelProperty, value);
        }

        private void OnChanceItem()
        {
            RaiseEvent(new RoutedEventArgs(PickItemCidadeEvent, this));
        }

        private ObservableCollection<Cidade> _listaCidade;
        private Cidade _cidadeSelecionado;

        public Cidade CidadeSelecionado
        {
            get => _cidadeSelecionado;
            set
            {
                if (Equals(value, _cidadeSelecionado)) return;
                _cidadeSelecionado = value;
                OnPropertyChanged();
                OnChanceItem();
            }
        }

        public ObservableCollection<Cidade> ListaCidade
        {
            get => _listaCidade;
            set
            {
                if (Equals(value, _listaCidade)) return;
                _listaCidade = value;
                OnPropertyChanged();
            }
        }

        private void PreencherListaCidade()
        {
            ListaCidade = new ObservableCollection<Cidade>();

            if (_cacheCidades == null)
            {
                _cacheCidades = NegocioCriador.CriaNegocioCidade().Lista();
            }
        }


        public ComboBoxCidade()
        {
            DataContext = this;
            InitializeComponent();
            PreencherListaCidade();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void PesquisaPorUf(params Uf[] ufs)
        {
            if (ufs == null || ufs.Length == 0)
            {
                return;
            }

            ListaCidade.Clear();


            IList<Cidade> cidadesFiltradas = ufs.SelectMany(uf => _cacheCidades.Where(c => c.Uf.Id == uf.Id)).ToList();

            var ufComboBoxDtos = cidadesFiltradas;

            foreach (var ufComboBoxDto in ufComboBoxDtos)
            {
                ListaCidade.Add(ufComboBoxDto);
            }

            CidadeSelecionado = ufComboBoxDtos[0];
        }
    }
}
