from sklearn.feature_extraction.text import TfidfVectorizer
import pandas as pd
from sklearn.feature_extraction.text import TfidfTransformer
from sklearn.feature_extraction.text import CountVectorizer
from nltk.stem import PorterStemmer
from heapq import nlargest
import nltk
from nltk.stem import WordNetLemmatizer
wordnet_lemmatizer = WordNetLemmatizer()
from nltk.corpus import wordnet
from nltk.corpus import stopwords
sw_nltk = stopwords.words('english')
##import spacy
##en = spacy.load('en_core_web_sm')
##sw_spacy = en.Defaults.stop_words
from nltk.tokenize import word_tokenize

def get_wordnet_pos(word):
    """Map POS tag to first character lemmatize() accepts"""
    tag = nltk.pos_tag([word])[0][1][0].upper()
    tag_dict = {"J": wordnet.ADJ,
                "N": wordnet.NOUN,
                "V": wordnet.VERB,
                "R": wordnet.ADV}

    return tag_dict.get(tag, wordnet.NOUN)

keyword = "apple"
return_sentences = []

sentences_file = open("grocery_store_sentences.txt", "r", encoding='utf-8-sig')
sentences_list = sentences_file.readlines()
stripped_sentences_list = []
returned_sentences_list = []
stemmed_sentence_list = []

punctuations="?:!.,;"
lemmatizer = WordNetLemmatizer()


for sentence in sentences_list:
    strip = sentence.strip()
    if keyword in strip:
        stripped_sentences_list.append(strip)

 

for sentence in stripped_sentences_list:
    words_to_join = []
    possible_adj = []
    #stripped_sentence = sentence.strip()
    stemmed_list = nltk.word_tokenize(sentence)
    for word in stemmed_list:
        if word in punctuations:
            stemmed_list.remove(word)
    tokens_without_stop = [word for word in stemmed_list if not word.lower() in sw_nltk]


    for word in tokens_without_stop:
        if get_wordnet_pos(word) == "r" and str(wordnet.synsets(word)[0])[:-5][-1] == 'r':
            for ss in wordnet.synsets(word):
                for lemmas in ss.lemmas(): # all possible lemmas
                    for ps in lemmas.pertainyms(): # all possible pertainyms
                        possible_adj.append(ps.name())
            for adj in possible_adj:
                if adj[0:3] == word[0:3]:
                    words_to_join.append(adj)
        else:
            words_to_join.append(lemmatizer.lemmatize(word, get_wordnet_pos(word)))

    lemmatized_output = ' '.join(words_to_join)        
    #lemmatized_output = ' '.join([lemmatizer.lemmatize(w, get_wordnet_pos(w)) for w in tokens_without_stop])
    stemmed_sentence_list.append(lemmatized_output)
    

#vectorizer = TfidfVectorizer()
#X = vectorizer.fit(stemmed_sentence_list)
#print(vectorizer.vocabulary_)
##
#### figure out how to strip all punctuation - maybe beforehand, when
#### reading the sentences from the file?
##
#### only 100 sentence dataset - maybe too small to accurately represent what
#### the proper distinguishing words are for each class?
##
##
###instantiate CountVectorizer() 
cv=CountVectorizer(token_pattern='(?u)\\b\\w+\\b') 
## 
### this steps generates word counts for the words in your docs 
word_count_vector=cv.fit_transform(stemmed_sentence_list)
tfidf_transformer=TfidfTransformer(smooth_idf=True,use_idf=True) 
tfidf_transformer.fit(word_count_vector)
df_idf = pd.DataFrame(tfidf_transformer.idf_, index=cv.get_feature_names(),columns=["idf_weights"]) 
## 
### sort ascending 
df_idf.sort_values(by=['idf_weights'])
top_words = list(df_idf.tail(5).index)
print(df_idf)
##
##unique_sentence_weights = {}
###for all the sentences containing the keyword                
##for basic_sent in returned_sentences_list:
##    unique_sentence_weights[basic_sent] = 0
##    #compute the sum of the idfs for a sentence with no duplicate words
##    #make the sentence unique first (remove duplicate words like "the")
##    basic_sent_split = basic_sent.lower().strip("?.’").split(" ")
##    for word in basic_sent_split:
##        if word not in unique_sentence_weights:
##            unique_sentence_weights[basic_sent] = unique_sentence_weights[basic_sent]+ float(df_idf.loc[[word.lower()], 'idf_weights'][word.lower()])
##        
##highest_sen = nlargest(4, unique_sentence_weights, key = unique_sentence_weights.get)              
##     
##
##sentences_file.close()
