using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using UAU.Fiscal.Document.Export.objects;
using Personal.AzureFramework;
using UAU.Fiscal.QueryService.domain;


namespace UAU.Fiscal.QueryService
{
    /// <summary>
    /// Realiza consultas no banco de dados sql server local
    /// </summary>
    public class SqlQuery
    {
        /// <summary>
        /// Consulta todos as restrições de bancos na base sql server
        /// </summary>
        /// <returns></returns>
        public async Task<List<RestricaoBancoConta>> FindRestricoesAsync()
        {

            string sql = "           SELECT Empresa_BcoCont, \n"
                        + "                  Banco_BcoCont, \n"
                        + "                  Conta_BcoCont \n"
                        + "           FROM   BancoContaUsuarios";

            List<RestricaoBancoConta> restricoes = new List<RestricaoBancoConta>();

            using (SqlConnection con = new SqlConnection(KeyVaultServiceQueues.Get().Configs["conectionstring-cosmos-exportador-contabil"].ToString()))
            {
                await con.OpenAsync();

                SqlCommand com = new SqlCommand(sql, con);
                com.CommandTimeout = 600;

                SqlDataReader reader = await com.ExecuteReaderAsync();

                long contador = 0;
                //Pegando a definição da tabela
                while (reader.Read())
                {


                    RestricaoBancoConta d = new RestricaoBancoConta()
                    {
                        id = (++contador).ToString(),
                        Empresa_BcoCont = long.Parse(reader["Empresa_BcoCont"].ToString()),
                        Banco_BcoCont = long.Parse(reader["Banco_BcoCont"].ToString()),
                        Conta_BcoCont = reader["Conta_BcoCont"].ToString(),



                    };
                    restricoes.Add(d);
                }

                reader.Close();

            }
            return restricoes;
        }


        /// <summary>
        /// Consulta todos os depósitos na base sql server
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProcessamentoDeposito>> FindDepositosByEmpresaAsync(long empresa)
        {
            string sql = $" \n"
               + "SELECT CASE 0 \n"
               + "            WHEN 0 THEN VwD.PercentJurCompSemProcDesc_Rpd \n"
               + "            ELSE VwD.PercentJurCompProcDesc_Rpd \n"
               + "       END                        AS PercentJurComp_Rpd, \n"
               + "       CASE 0 \n"
               + "            WHEN 0 THEN VwD.PercentDescAntecSemProcDesc_Rpd \n"
               + "            ELSE VwD.PercentDescAntecProcDesc_Rpd \n"
               + "       END                        AS PercentDescAntec_Rpd, \n"
               + "       CASE 0 \n"
               + "            WHEN 2 THEN VwD.Data_doc \n"
               + "            ELSE VwD.DataContabil_Rpd \n"
               + "       END                        AS DataContabil, \n"
               + "       Numero_Dep, \n"
               + "       NumReceb_Rpd, \n"
               + "       NumCont_Rpd, \n"
               + "       TipoRpg_rpd, \n"
               + "       Banco_Dep, \n"
               + "       Conta_Dep, \n"
               + "       CategMovFin_Dep, \n"
               + "       HistLanc_Dep, \n"
               + "       ObraFiscal_Rpd, \n"
               + "       Empresa_rpg, \n"
               + "       IntExt_Vrec, \n"
               + "       AlmoxCentral_VRec, \n"
               + "       Status_Rpg, \n"
               + "       Conciliado_Dep, \n"
               + "       Data_doc, \n"
               + "       Cliente_Rpg, \n"
               + "       Nome_pes, \n"
               + "       VwD.Fisc_obr, \n"
               + "       VwD.FiscRec_obr, \n"
               + "       VwD.TipoObra_obr, \n"
               + "       NomeBanco, \n"
               + "       NomeTitular, \n"
               + "       DataVenci_Rec, \n"
               + "       TotParcGrupo_rec, \n"
               + "       Descricao_Par, \n"
               + "       Tipo_par, \n"
               + "       PercentValor_Rpd, \n"
               + "       PercentJurCompSemProcDesc_Rpd, \n"
               + "       PercentDescAntecSemProcDesc_Rpd, \n"
               + "       PercentJurCompProcDesc_Rpd, \n"
               + "       PercentDescAntecProcDesc_Rpd, \n"
               + "       PercentPrinc_Rpd           AS PercentPrinc_Rpd, \n"
               + "       PercentCorr_Rpd, \n"
               + "       PercentCorrAtr_Rpd, \n"
               + "       PercentTaxaBol_Rpd, \n"
               + "       PercentMultaAtr_Rpd, \n"
               + "       PercentJurAtr_Rpd, \n"
               + "       PercentAcres_Rpd, \n"
               + "       PercentDescontoCusta_Rpd, \n"
               + "       PercentDesc_Rpd, \n"
               + "       PercentDescontoCondicional_rpd, \n"
               + "       HistLancRec_Rpd, \n"
               + "       Empresa_rpd, \n"
               + "       Obra_rpd, \n"
               + "       NumParc_Rpd, \n"
               + "       NumVend_Rpd, \n"
               + "       DataContabil_Rpd, \n"
               + "       Cap_Rpd, \n"
               + "       CapJuros_Rpd, \n"
               + "       CapCorrecao_Rpd, \n"
               + "       CapMulta_Rpd, \n"
               + "       CapJurosAtr_Rpd, \n"
               + "       CapAcrescimo_Rpd, \n"
               + "       CapDesconto_Rpd, \n"
               + "       CapDescontoCondicional_Rpd, \n"
               + "       CapCorrecaoAtr_Rpd, \n"
               + "       CapRepasse_rpd, \n"
               + "       CapTaxaBol_Rpd, \n"
               + "       CapDescontoAntec_Rpd, \n"
               + "       CapDescontoCusta_Rpd, \n"
               + "       Desc_emp, \n"
               + "       Descr_obr \n"
               + "FROM   VwProcLancFiscalDepositos  AS VwD WITH (NOLOCK) \n"
               + "       INNER JOIN Empresas WITH (NOLOCK) \n"
               + "            ON  Codigo_emp = Empresa_rpd \n"
               + "       INNER JOIN Obras WITH (NOLOCK) \n"
               + "            ON  Empresa_obr = Empresa_rpd \n"
               + "            AND Cod_obr = Obra_rpd \n"
               + $"WHERE  VwD.Conciliado_Dep = 1 AND Empresas.Codigo_emp = {empresa} \n"
               + "       AND ( \n"
               + "               VwD.IntExt_VRec = 'E' \n"
               + "               OR (VwD.IntExt_Vrec = 'I' AND VwD.AlmoxCentral_VRec = 1) \n"
               + "           ) \n"
               + "       AND ( \n"
               + "               VwD.Status_Rpg = 1 \n"
               + "               OR (VwD.Status_Rpg = 2 AND VwD.Empresa_rpde IS NOT NULL) \n"
               + "           ) \n"
               + "       AND VwD.Banco_Dep IS NOT NULL";

            List<ProcessamentoDeposito> depositos = new List<ProcessamentoDeposito>();

            using (SqlConnection con = new SqlConnection(KeyVaultServiceQueues.Get().Configs["conectionstring-cosmos-exportador-contabil"].ToString()))
            {

                await con.OpenAsync();

                SqlCommand com = new SqlCommand(sql, con);

                SqlDataReader reader = await com.ExecuteReaderAsync();
                long contador = 0;

                //Pegando a definição da tabela
                while (reader.Read())
                {


                    ProcessamentoDeposito d = new ProcessamentoDeposito()
                    {
                        id = (++contador).ToString(),
                        AlmoxCentral_VRec = int.Parse(reader["AlmoxCentral_VRec"].ToString()),
                        Banco_Dep = int.Parse(reader["Banco_Dep"].ToString()),
                        FiscRec_obr = reader["FiscRec_obr"].ToString(),
                        Empresa_rpg = int.Parse(reader["Empresa_rpg"].ToString()),
                        CapAcrescimo_Rpd = reader["CapAcrescimo_Rpd"].ToString(),
                        CapCorrecaoAtr_Rpd = reader["CapCorrecaoAtr_Rpd"].ToString(),
                        CapCorrecao_Rpd = reader["CapCorrecao_Rpd"].ToString(),
                        CapDescontoAntec_Rpd = reader["CapDescontoAntec_Rpd"].ToString(),
                        CapDescontoCondicional_Rpd = reader["CapDescontoCondicional_Rpd"].ToString(),
                        CapDescontoCusta_Rpd = reader["CapDescontoCusta_Rpd"].ToString(),
                        CapDesconto_Rpd = reader["CapDesconto_Rpd"].ToString(),
                        CapJurosAtr_Rpd = reader["CapJurosAtr_Rpd"].ToString(),
                        CapJuros_Rpd = reader["CapJuros_Rpd"].ToString(),
                        CapMulta_Rpd = reader["CapMulta_Rpd"].ToString(),
                        CapRepasse_rpd = reader["CapRepasse_rpd"].ToString(),
                        CapTaxaBol_Rpd = reader["CapTaxaBol_Rpd"].ToString(),
                        Cap_Rpd = reader["Cap_Rpd"].ToString(),
                        PercentAcres_Rpd = Decimal.Parse(reader["PercentAcres_Rpd"].ToString()),
                        PercentCorrAtr_Rpd = Decimal.Parse(reader["PercentCorrAtr_Rpd"].ToString()),
                        PercentCorr_Rpd = Decimal.Parse(reader["PercentCorr_Rpd"].ToString()),
                        PercentDescAntecProcDesc_Rpd = Decimal.Parse(reader["PercentDescAntecProcDesc_Rpd"].ToString()),
                        PercentDescAntecSemProcDesc_Rpd = Decimal.Parse(reader["PercentDescAntecSemProcDesc_Rpd"].ToString()),
                        PercentDescAntec_Rpd = Decimal.Parse(reader["PercentDescAntec_Rpd"].ToString()),
                        PercentJurAtr_Rpd = Decimal.Parse(reader["PercentJurAtr_Rpd"].ToString()),
                        PercentDesc_Rpd = Decimal.Parse(reader["PercentDesc_Rpd"].ToString()),
                        PercentJurComp_Rpd = Decimal.Parse(reader["PercentJurComp_Rpd"].ToString()),
                        PercentDescontoCondicional_rpd = Decimal.Parse(reader["PercentDescontoCondicional_rpd"].ToString()),
                        PercentJurCompProcDesc_Rpd = Decimal.Parse(reader["PercentJurCompProcDesc_Rpd"].ToString()),
                        PercentJurCompSemProcDesc_Rpd = Decimal.Parse(reader["PercentJurCompSemProcDesc_Rpd"].ToString()),
                        PercentDescontoCusta_Rpd = Decimal.Parse(reader["PercentDescontoCusta_Rpd"].ToString()),
                        PercentMultaAtr_Rpd = Decimal.Parse(reader["PercentMultaAtr_Rpd"].ToString()),
                        PercentPrinc_Rpd = Decimal.Parse(reader["PercentPrinc_Rpd"].ToString()),
                        PercentTaxaBol_Rpd = Decimal.Parse(reader["PercentTaxaBol_Rpd"].ToString()),
                        PercentValor_Rpd = Decimal.Parse(reader["PercentValor_Rpd"].ToString()),
                        CategMovFin_Dep = reader["CategMovFin_Dep"].ToString(),
                        Cliente_Rpg = int.Parse(reader["Cliente_Rpg"].ToString()),
                        Conciliado_Dep = int.Parse(reader["Conciliado_Dep"].ToString()),
                        Conta_Dep = reader["Conta_Dep"].ToString(),
                        DataContabil = DateTime.Parse(reader["DataContabil"].ToString()),
                        DataContabil_Rpd = DateTime.Parse(reader["DataContabil_Rpd"].ToString()),
                        DataVenci_Rec = DateTime.Parse(reader["DataVenci_Rec"].ToString()),
                        Data_doc = DateTime.Parse(reader["Data_doc"].ToString()),
                        Descricao_Par = reader["Descricao_Par"].ToString(),
                        Descr_obr = reader["Descr_obr"].ToString(),
                        Desc_emp = reader["Desc_emp"].ToString(),
                        Empresa_rpd = int.Parse(reader["Empresa_rpd"].ToString()),
                        Fisc_obr = reader["Fisc_obr"].ToString(),
                        HistLancRec_Rpd = reader["HistLancRec_Rpd"].ToString(),
                        HistLanc_Dep = reader["HistLanc_Dep"].ToString(),
                        IntExt_Vrec = reader["IntExt_Vrec"].ToString(),
                        NomeBanco = reader["NomeBanco"].ToString(),
                        NomeTitular = reader["NomeTitular"].ToString(),
                        Nome_pes = reader["Nome_pes"].ToString(),
                        NumCont_Rpd = int.Parse(reader["NumCont_Rpd"].ToString()),
                        Numero_Dep = int.Parse(reader["Numero_Dep"].ToString()),
                        NumParc_Rpd = int.Parse(reader["NumParc_Rpd"].ToString()),
                        NumReceb_Rpd = int.Parse(reader["NumReceb_Rpd"].ToString()),
                        NumVend_Rpd = int.Parse(reader["NumVend_Rpd"].ToString()),
                        ObraFiscal_Rpd = reader["ObraFiscal_Rpd"].ToString(),
                        Obra_rpd = reader["Obra_rpd"].ToString(),
                        Status_Rpg = int.Parse(reader["Status_Rpg"].ToString()),
                        TipoObra_obr = int.Parse(reader["TipoObra_obr"].ToString()),
                        TipoRpg_rpd = reader["TipoRpg_rpd"].ToString(),
                        Tipo_par = reader["Tipo_par"].ToString(),
                        TotParcGrupo_rec = int.Parse(reader["TotParcGrupo_rec"].ToString())



                    };
                    depositos.Add(d);
                }

                reader.Close();

            }
            return depositos;
        }

    }
        

}
