public class Trie
{
    private readonly TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }

    public void Insert(string word)
    {
        var currentNode = root;
        foreach (var letter in word)
        {
            if (!currentNode.Children.ContainsKey(letter))
            {
                currentNode.Children[letter] = new TrieNode();
            }
            currentNode = currentNode.Children[letter];
        }
        currentNode.IsEndOfWord = true;
    }

    public bool Search(string word)
    {
        var currentNode = root;
        foreach (var letter in word)
        {
            if (!currentNode.Children.ContainsKey(letter))
            {
                return false;
            }
            currentNode = currentNode.Children[letter];
        }
        return currentNode.IsEndOfWord;
    }

    public bool StartsWith(string prefix)
    {
        var currentNode = root;
        foreach (var letter in prefix)
        {
            if (!currentNode.Children.ContainsKey(letter))
            {
                return false;
            }
            currentNode = currentNode.Children[letter];
        }
        return true;
    }
}
