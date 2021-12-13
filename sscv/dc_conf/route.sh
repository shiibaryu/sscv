declare -a array=("S1" "R1" "R2" "R3" "R4" "Firewall" "Nat" "D1" "D2")

for ((i = 0; i < ${#array[@]}; i++)) {
	#mkdir ${array[i]}
	docker exec -it ${array[i]} ip -6 route show table all > ./${array[i]}/v6route.txt
	docker exec -it ${array[i]} ip -j addr > ./${array[i]}/if.json
}
