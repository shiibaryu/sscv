#include <iostream>
#include <fstream>
#include <filesystem>
#include "nlohmann/json.hpp"
//#include "ddlog.h"

namespace nl = nlohmann;
using std::__fs::filesystem::directory_iterator;

struct interface_property{
	std::string state;
	int mtu;
	std::string name;
	std::string owner;
	int port_number;
	char eth_addr[48];
	std::string ipv4_addr;
	std::string ipv6_addr_global;
	std::string ipv6_addr_host;
	std::string ipv6_addr_linklocal;
	std::string ipv6_addr_local;
};

struct route_property{
	std::string ip_addr;
	std::string interface_name;
	std::string nexthop_device;
};

struct device_property{
	std::string name;
	struct interface_property infp;
	struct route_property ipv4_route;
	struct route_property ipv6_route;
};

void get_interface_property(std::string if_path,struct interface_property *inf_prop)
{
	//const std::string pathToJson(if_path);
	const std::string pathToJson("./R1/if.json");
	std::ifstream ifs(pathToJson.c_str());

	inf_prop->state = ;
	inf_prop->mtu = ;
	inf_prop->owner = ;
	inf_prop->port_number = ;
	memcpy(inf_prop->eth_addr,,48);
	inf_prop->ipv4_addr = ;
	inf_prop->ipv6_addr_global = ;
	inf_prop->ipv6_addr_host = ;
	inf_prop->ipv6_addr_linklocal = ;
	inf_prop->ipv6_addr_local = ;

	//apply commit
}


	/*
	if(ifs.good()){
		nl::json j;
		ifs >> j;
		for (const auto &elem : j.items()){
            std::cout << elem.value() << std::endl;
        }
	}
	*/

void get_interface_properties(std::vector<std::string> if_path)
{
	struct interface_property inf_prop;
	for(int i=0;i<if_path.size();i++){
		get_interface_property(if_path[i],&inf_prop);
	}
	//do commit
}

void get_v4route_property(std::string route_path)
{
	//fileの長さ数える
	//ルートの数を算出
	//while回す
	//積める	
}

void get_v4route_properties(std::vector<std::string> route_path)
{
	for(int i=0;i<route_path.size();i++){
		get_v4route_property(route_path[i]);
	}
}

void get_v6route_property(std::string route_path)
{
	//fileの長さ数える
	//ルートの数を算出
	//while回す
	//積める	
}

void get_v6route_properties(std::vector<std::string> route_path)
{
	for(int i=0;i<route_path.size();i++){
		get_v6route_property(route_path[i]);
	}
}

std::vector<std::string> get_directories(const std::string& s)
{
    std::vector<std::string> r;
    for(auto& p : std::__fs::filesystem::recursive_directory_iterator(s))
        if(p.is_directory()){
			if(p.path().string().find("topology") == std::string::npos){
				r.push_back(p.path().string());
			}
		}
    return r;
}

int main(int args,char **argv)
{
	std::string path = "/Users/siiba/Program/batzen/Zen/batzenv6Comp/conf/";
	std::vector<std::string> if_path = get_directories(path);
	std::vector<std::string> routev4_path = get_directories(path);
	std::vector<std::string> routev6_path = get_directories(path);

	for(auto i = if_path.begin();i != if_path.end();i++){
		*i += "/if.json";
	}

	for(auto i = routev4_path.begin();i != routev4_path.end();i++){
		*i += "/v4route.txt";
	}

	for(auto i = routev6_path.begin();i != routev6_path.end();i++){
		*i += "/v6route.txt";
	}
	
	//process if
	get_interface_properties(if_path);	

	//process v4route
	get_v4route_properties(routev4_path);

	//process v6route
	get_v6route_properties(routev6_path);

    return 0;
	/*
	ddlog_prog prog = ddlog_run(1,true,NULL,NULL);
	if(prog == NULL){
		fprintf(stderr,"failed to initialize ddlog program");
		exit(EXIT_FAILURE);
	}

	table_id tableID = ddlog_get_table_id(prog,"");
	printf("table id is %lu\n",tableID);
	*/
	//creating record values in the ddlog format
	//ddlog_record *src = ddlog_string(???);
	//ddlog_record *dst = ddlog_string(???);
	//ddlog_record *lstatus = ddlog_bool(??);
	//ddlog_record *new_record = ddlog_struct("???",struct_args,number_of_args);
	//
	//if(ddlog_transaction_start(prog) < 0){}
	//ddlog_cmd *cmd = ddlog_insert_cmd(tableID,new_record);
	//
	//if(ddlog_apply_updates(prog,&cmd,1)<0)
	//
	//if(ddlog_transaction_commit(prog)<0)
	//
	//ddlog_stop(prog) < 0
}

