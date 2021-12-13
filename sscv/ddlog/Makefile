CC = g++
INCLUDE := -I../lib/rapidjson/include/rapidjson

PROGNAME = main

all: $(PROGNAME)

.c.o:
	$(CC) $< -o $@

clean:
	rm -rf *.o
	rm -rf $(PROGNAME)
