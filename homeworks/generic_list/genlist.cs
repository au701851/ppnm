public class genlist<T> {
	public T[] data; // Creates array of elements of type T

	//properties
	//public int size => data.Length; // size redirects to data.Length
	public int size = 0, capacity = 8; //The list has at first length 0, and capacity 8
	public T this[int i] => data[i]; //calling genlist[n] redirects to data[n]

	//Constructor
	public genlist(){
		//data = new T[0]; // Creates a list of length zero
		data = new T[capacity]; //To improve speed, I initially make the list longer, and double its size when I need it
	}

	public void add(T item){
		if(size == capacity){ //We need a longer list
			T[] newdata = new T[capacity*=2]; //Makes a new, longer array, with room for the new element
			System.Array.Copy(data, newdata, size); //Copies the array
			data = newdata; //Overrides old data array -- which is now only necessary if we exceed the capacity of the old list}
			}
		data[size] = item; //item is added as the first available slot of the list
		size++;
	}
	public void remove(T item){
		T[] newdata = new T[capacity];
		bool found = false;
		int j = 0; //I need two iteration variables to avoid empty spots in my list. 
		for(int i = 0; i<size; i++){
			if(data[i].Equals(item)) {found = true;} //The unwanted item is not added to the list, but diminishes its size by one. 
			else{
				newdata[j] = data[i];
				j++;
			}
		}
		if(found) size--;
		data = newdata;
	}
}
