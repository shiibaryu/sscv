import numpy as np
import pandas as pd
import string
from pybatfish.client.commands import *
from pybatfish.datamodel import *
from pybatfish.datamodel.answer import *
from pybatfish.datamodel.flow import *
from pybatfish.question import *
from pybatfish.question import bfq

if __name__ == '__main__':

    # connecting the batfish with this script
    bf_session.host = 'localhost'

    # add a name of the network
    bf_set_network('example_nw')

    # packaging the snapshot data
    snap = '../networks/example/live/'
    bf_init_snapshot(snap,name='example',overwrite=True)

    # load questions
    load_questions()

    conf = bfq.interfaceProperties().answer().frame()
    conf = conf.sort_values('Interface')
    conf.to_csv("./conf.csv");

    l3 = bfq.layer3Edges().answer().frame();
    l3 = l3.sort_values('Interface')
    l3.to_csv("./l3.csv");


    routes = bfq.routes().answer().frame();
    routes = routes.sort_values(["Node","Admin_Distance","Metric"]);
    routes.to_csv("./l.csv");


    #print(result)


    print("Hello")
