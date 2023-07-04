# JsonParser

- For this solution, I took a Json File as a Tree. Taking the primitives properties as a final node and the Objects "{}" or Arrays "[]" as parents nodes.
	- 1 Reads the input Json as a JToken as first Parent Node.
	- 2 Then goes iterative through this JToken, checking if it's a parent node (object or array *). 
		- 2a If it is a parent node, then goes through this inner tree.
		- 2b If it is not, it takes the nodes, removes its parent, and adds it again at the beginning of the output object.
	
*Object and Array are handled differently with similar structures, since an Object has properties, and an Array has primitives values or Objects.