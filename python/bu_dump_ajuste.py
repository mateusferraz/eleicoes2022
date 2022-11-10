import argparse
import io,json
import logging
import os
import sys

import asn1tools

jsonDados = ""

def espacos(profundidade: int):
    return ".   " * profundidade


def valor_membro(membro):
    if isinstance(membro, (bytes, bytearray)):
        return bytes(membro).hex()
    return membro


def print_list(lista: list, profundidade: int):
    indent = espacos(profundidade)
    for membro in lista:
        if type(membro) is dict:
            print_dict(membro, profundidade + 1)
        else:
            jsonDados += (f"{indent}valor_membro(membro)")
            # print(f"{indent}valor_membro(membro)")


def print_dict(entidade: dict, profundidade: int):
    indent = ""#espacos(profundidade)    
    global jsonDados
    # if 'resultadosVotacaoPorEleicao' in entidade:
    # teste = entidade["resultadosVotacaoPorEleicao"]["resultadosVotacao"]["totaisVotosCargo"]
    for key in sorted(entidade):
        membro = entidade[key]
        if (key == "assinatura"):
            data= 1
        if type(membro) is dict:
            #print(f"\"{indent}{key}\":")
            jsonDados += (f"\"{indent}{key}\":")            
            # print("{")
            jsonDados +="{"
            print_dict(membro, profundidade + 1)
            # print("},")
            jsonDados +="},"
            #print(json.dumps(entidade))
        elif type(membro) is list:
            # print(f"\"{indent}{key}\": [")
            jsonDados += (f"\"{indent}{key}\": [")            
            # print("{")
            jsonDados +="{"
            print_list(membro, profundidade + 1)
            # print("}")
            jsonDados +="}"
            # print(f"{indent}],")
            jsonDados +=(f"{indent}],")
        else:
            # print(f"\"{indent}{key}\" :\"{valor_membro(membro)}\",")
            jsonDados +=(f"\"{indent}{key}\" :\"{valor_membro(membro)}\",")

def processa_bu(asn1_paths: list, bu_path: str):
    conv = asn1tools.compile_files(asn1_paths, codec="ber")
    with open(bu_path, "rb") as file:
        envelope_encoded = bytearray(file.read())
    envelope_decoded = conv.decode("EntidadeEnvelopeGenerico", envelope_encoded)
    bu_encoded = envelope_decoded["conteudo"]
    del envelope_decoded["conteudo"]  # remove o conteúdo para não imprimir como array de bytes
    global jsonDados
    jsonDados += "{"
    jsonDados += "\"EntidadeEnvelopeGenerico\":{"
    # print("{")
    # print("\"EntidadeEnvelopeGenerico\":{")
    print_dict(envelope_decoded, 1)
    #print("},")
    jsonDados += "},"
    bu_decoded = conv.decode("EntidadeBoletimUrna", bu_encoded)
    #print("\"EntidadeBoletimUrna\":{")
    jsonDados += "\"EntidadeBoletimUrna\":{"
    print_dict(bu_decoded, 1)
    # print("}")
    # print("}")
    jsonDados += "}"
    jsonDados += "}"
    print(jsonDados)

    with io.open('data.json', 'w', encoding='utf-8') as outfile:
        outfile.write(jsonDados)

def main():
    
    parser = argparse.ArgumentParser(
        description="Converte um Boletim de Urna (BU) da Urna Eletrônica (UE) e imprime um extrato",
        formatter_class=argparse.RawTextHelpFormatter)
    parser.add_argument("-a", "--asn1", nargs="+", required=False,
                        help="Caminho para o arquivo de especificação asn1 do BU")
    parser.add_argument("-b", "--bu", type=str, required=False,
                        help="Caminho para o arquivo de BU originado na UE")
    parser.add_argument("--debug", action="store_true", help="ativa o modo DEBUG do log")

    args = parser.parse_args()

    bu_path = args.bu
    asn1_paths = args.asn1
    # bu_path = "C:\\Users\\mfsantos10\\Downloads\\o00406-6373800430007.bu"
    # asn1_paths = "C:\\Users\\mfsantos10\\Documents\\MATEUS\BU\\formato-arquivos-bu-rdv-ass-digital\\spec\\bu.asn1"
    level = logging.DEBUG if args.debug else logging.INFO
    logging.basicConfig(level=level, format="%(asctime)s - %(levelname)s - %(message)s")

    logging.info("Converte %s com as especificações %s", bu_path, asn1_paths)
    if not os.path.exists(bu_path):
        logging.error("Arquivo do BU (%s) não encontrado", bu_path)
        sys.exit(-1)
    for asn1_path in asn1_paths:
        if not os.path.exists(asn1_path):
            logging.error("Arquivo de especificação do BU (%s) não encontrado", asn1_path)
            sys.exit(-1)

    processa_bu(asn1_paths, bu_path)


if __name__ == "__main__":
    main()
