using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAU.Fiscal.Document.Export.objects
{
    public class ProcessamentoDeposito
    {
        public string id { get; set; }
        public Decimal PercentJurComp_Rpd { get; set; }
        public Decimal PercentDescAntec_Rpd { get; set; }
        public DateTime DataContabil { get; set; }
        public int Numero_Dep { get; set; }
        public int NumReceb_Rpd { get; set; }
        public int NumCont_Rpd { get; set; }
        public string TipoRpg_rpd { get; set; }
        public int Banco_Dep { get; set; }
        public string Conta_Dep { get; set; }
        public string CategMovFin_Dep { get; set; }
        public string HistLanc_Dep { get; set; }
        public string ObraFiscal_Rpd { get; set; }
        public int Empresa_rpg { get; set; }
        public string IntExt_Vrec { get; set; }
        public int AlmoxCentral_VRec { get; set; }
        public int Status_Rpg { get; set; }
        public int Conciliado_Dep { get; set; }
        public DateTime Data_doc { get; set; }
        public int Cliente_Rpg { get; set; }
        public string Nome_pes { get; set; }
        public string Fisc_obr { get; set; }
        public string FiscRec_obr { get; set; }
        public int TipoObra_obr { get; set; }
        public string NomeBanco { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataVenci_Rec { get; set; }
        public int TotParcGrupo_rec { get; set; }
        public string Descricao_Par { get; set; }
        public string Tipo_par { get; set; }
        public Decimal PercentValor_Rpd { get; set; }
        public Decimal PercentJurCompSemProcDesc_Rpd { get; set; }
        public Decimal PercentDescAntecSemProcDesc_Rpd { get; set; }
        public Decimal PercentJurCompProcDesc_Rpd { get; set; }
        public Decimal PercentDescAntecProcDesc_Rpd { get; set; }
        public Decimal PercentPrinc_Rpd { get; set; }
        public Decimal PercentCorr_Rpd { get; set; }
        public Decimal PercentCorrAtr_Rpd { get; set; }
        public Decimal PercentTaxaBol_Rpd { get; set; }
        public Decimal PercentMultaAtr_Rpd { get; set; }
        public Decimal PercentJurAtr_Rpd { get; set; }
        public Decimal PercentAcres_Rpd { get; set; }
        public Decimal PercentDescontoCusta_Rpd { get; set; }
        public Decimal PercentDesc_Rpd { get; set; }
        public Decimal PercentDescontoCondicional_rpd { get; set; }
        public string HistLancRec_Rpd { get; set; }
        public int Empresa_rpd { get; set; }
        public string Obra_rpd { get; set; }
        public int NumParc_Rpd { get; set; }
        public int NumVend_Rpd { get; set; }
        public DateTime DataContabil_Rpd { get; set; }
        public string Cap_Rpd { get; set; }
        public string CapJuros_Rpd { get; set; }
        public string CapCorrecao_Rpd { get; set; }
        public string CapMulta_Rpd { get; set; }
        public string CapJurosAtr_Rpd { get; set; }
        public string CapAcrescimo_Rpd { get; set; }
        public string CapDesconto_Rpd { get; set; }
        public string CapDescontoCondicional_Rpd { get; set; }
        public string CapCorrecaoAtr_Rpd { get; set; }
        public string CapRepasse_rpd { get; set; }
        public string CapTaxaBol_Rpd { get; set; }
        public string CapDescontoAntec_Rpd { get; set; }
        public string CapDescontoCusta_Rpd { get; set; }
        public string Desc_emp { get; set; }
        public string Descr_obr { get; set; }
    }
    }
