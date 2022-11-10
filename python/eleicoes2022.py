from fileinput import filename
from unittest import result
import asn1tools
import io,json
import csv
import collections
import argparse
import logging
import os
import sys
import shutil
from glob import glob



def process_bu(bu_path: str, conv):
    with open(bu_path, 'rb') as f:
        envelope_encoded = bytearray(f.read())
    envelope_decoded = conv.decode('EntidadeEnvelopeGenerico', envelope_encoded)
    bu_encoded = envelope_decoded['conteudo']
    bu_decoded = conv.decode('EntidadeBoletimUrna', bu_encoded)    
    return bu_decoded

   

def savarArquivoCSV(data: dict):
    local = 'C:/mateus/repos/mateus/Eleicao2022/BUs/csv/'
    fileName=data[0]["municipio"]+"_"+data[0]["zona"]+"_"+data[0]["secao"]+".csv"
    cabecalho = ["dataGeracao","idEleitoral","chaveAssinaturaVotosVotavel","dadosSecaoSA","dataHoraEmissao","fase","codigoCarga","dataHoraCarga","numeroInternoUrna","numeroSerieFC","identificacao","numeroSerieFV","tipoArquivo","tipoUrna","versaoVotacao","local","municipio","zona","secao","qtdEleitoresCompBiometrico","qtdEleitoresLibCodigo","idEleicao1","qtdEleitoresAptos1","qtdComparecimento1","tipoCargo1","idEleicao2","qtdEleitoresAptos2","qtdComparecimento2","tipoCargo2","codigoCargo","ordemImpressao","assinatura","partido","quantidadeVotos","tipoVoto"]
    with io.open((local+fileName), 'w',newline='', encoding='utf-8') as outfile:
        writer = csv.DictWriter(outfile, fieldnames=cabecalho,delimiter=";")
        writer.writeheader()
        #for cIndex,cValue in enumerate(data):                        
        for index in range(len(data)):
            writer.writerow(data[index])

def prepararDados(data: dict):    
    lista =dict()
    listaTodos =dict()    
    lista["dataGeracao"] = data["cabecalho"]["dataGeracao"]
    lista["idEleitoral"] = str(data["cabecalho"]["idEleitoral"])
    lista["chaveAssinaturaVotosVotavel"] = str(valor_membro(data["chaveAssinaturaVotosVotavel"]))
    lista["dadosSecaoSA"] = str(data["dadosSecaoSA"])
    lista["dataHoraEmissao"]= data["dataHoraEmissao"]
    lista["fase"] = data["fase"]
    lista["codigoCarga"] = valor_membro(data["urna"]["correspondenciaResultado"]["carga"]["codigoCarga"])
    lista["dataHoraCarga"] = data["urna"]["correspondenciaResultado"]["carga"]["dataHoraCarga"]
    lista["numeroInternoUrna"] = str(data["urna"]["correspondenciaResultado"]["carga"]["numeroInternoUrna"])
    lista["numeroSerieFC"] = str(valor_membro(data["urna"]["correspondenciaResultado"]["carga"]["numeroSerieFC"]))
    lista["identificacao"] = str(data["urna"]["correspondenciaResultado"]["identificacao"])
    lista["numeroSerieFV"] = str(valor_membro(data["urna"]["numeroSerieFV"]))
    lista["tipoArquivo"] = data["urna"]["tipoArquivo"]
    lista["tipoUrna"] = data["urna"]["tipoUrna"]
    lista["versaoVotacao"] = data["urna"]["versaoVotacao"]
    lista["local"] = str(data["identificacaoSecao"]["local"])
    lista["municipio"] = str(data["identificacaoSecao"]["municipioZona"]["municipio"])
    lista["zona"] = str(data["identificacaoSecao"]["municipioZona"]["zona"])
    lista["secao"]= str(data["identificacaoSecao"]["secao"])   
    if 'qtdEleitoresCompBiometrico' in data:
        lista["qtdEleitoresCompBiometrico"] = data["qtdEleitoresCompBiometrico"]
    else:
        lista["qtdEleitoresCompBiometrico"] = "null"
    
    if 'qtdEleitoresLibCodigo' in  data:
        lista["qtdEleitoresLibCodigo"] = str(data["qtdEleitoresLibCodigo"])
    else:
        lista["qtdEleitoresLibCodigo"] = 'null'
    lista["idEleicao1"] = str(data["resultadosVotacaoPorEleicao"][0]["idEleicao"])
    lista["qtdEleitoresAptos1"] = str(data["resultadosVotacaoPorEleicao"][0]["qtdEleitoresAptos"])
    lista["qtdComparecimento1"] = str(data["resultadosVotacaoPorEleicao"][0]["resultadosVotacao"][0]["qtdComparecimento"])
    lista["tipoCargo1"] = data["resultadosVotacaoPorEleicao"][0]["resultadosVotacao"][0]["tipoCargo"]
    #lista["#totaisVotosCargo"] = data["resultadosVotacaoPorEleicao"][0]["resultadosVotacao"]["totaisVotosCargo"]#tentar converter em json
    lista["idEleicao2"] = str(data["resultadosVotacaoPorEleicao"][1]["idEleicao"])
    lista["qtdEleitoresAptos2"] = str(data["resultadosVotacaoPorEleicao"][1]["qtdEleitoresAptos"])
    lista["qtdComparecimento2"] = str(data["resultadosVotacaoPorEleicao"][1]["resultadosVotacao"][0]["qtdComparecimento"])
    lista["tipoCargo2"] = data["resultadosVotacaoPorEleicao"][1]["resultadosVotacao"][0]["tipoCargo"]
    lista["codigoCargo"] = str(data["resultadosVotacaoPorEleicao"][1]["resultadosVotacao"][0]["totaisVotosCargo"][0]["codigoCargo"])
    lista["ordemImpressao"] = str(data["resultadosVotacaoPorEleicao"][1]["resultadosVotacao"][0]["totaisVotosCargo"][0]["ordemImpressao"])

    votos=data["resultadosVotacaoPorEleicao"][1]["resultadosVotacao"][0]["totaisVotosCargo"][0]["votosVotaveis"]  
    for index in range(len(votos)):
        tipoVoto = str(votos[index]["tipoVoto"])
        lista["tipoVoto"] = tipoVoto
        lista["assinatura"] = str(valor_membro(votos[index]["assinatura"]))
        if tipoVoto == "nominal" :            
            lista["partido"] = str(votos[index]["identificacaoVotavel"]["partido"])
        else:            
            lista["partido"] = str(votos[index]["tipoVoto"])
            
        lista["quantidadeVotos"] = str(votos[index]["quantidadeVotos"])

        if index not in listaTodos.keys():
            listaTodos[index] = dict(lista)


    savarArquivoCSV(listaTodos)

def valor_membro(membro):
    if isinstance(membro, (bytes, bytearray)):
        return bytes(membro).hex()
    return membro




def main():
    
    conv = asn1tools.compile_files('C:/mateus/repos/mateus/Eleicao2022/python/bu.asn1', codec="ber")
    # folder = 'C:/Users/mfsantos10/Documents/MATEUS/BU/kaggle/archive/bus/bu'
    folder = 'C:/Users/mfsantos10/Documents/MATEUS/BU/kaggle/files/bu'
    folderSuccess = 'C:/Users/mfsantos10/Documents/MATEUS/BU/kaggle/archive/bus/bu/Processado/'
    
    files_ = glob(folder+'/*')
    filename =""
    #folder = 'C:/mateus/repos/mateus/Eleicao2022/BUs/bu'
    #folder = 'C:/Users/mfsantos10/Documents/MATEUS/BU/kaggle/archive/bus/bu/o00406-5601400080001.bu'    
    for bu_file in files_:
        try:
            filename =bu_file.replace(folder,"")
            filename =filename.replace("\\","")
            data = process_bu(bu_file, conv)
            # partido1 = data["resultadosVotacaoPorEleicao"][1]["resultadosVotacao"][0]["totaisVotosCargo"][0]["votosVotaveis"][0]["identificacaoVotavel"]["partido"]
            # votos = data["resultadosVotacaoPorEleicao"][1]["resultadosVotacao"][0]["totaisVotosCargo"][0]["votosVotaveis"][0]["quantidadeVotos"]
            # data.replace("(","[")
            #result = json.dumps(data)
            prepararDados(data)
            # shutil.move(bu_file,(folderSuccess+'/'+filename)) 
            #print(data)
            #break
        except:            
            #shutil.move(bu_file,folder+'//erro//'+filename) 
            print('Erro processar :'+bu_file)
        


if __name__ == "__main__":
    main()