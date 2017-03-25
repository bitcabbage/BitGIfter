# BitGifter
BitGifter is a nice and sexy web service for buying giftcards.


## API
API is exposed through the following endpoint – 
> `http://localhost:62271/api/giftcards/`

The endpoint is protected by the **authentication token**. The token must be provided via `x-api-key` header.

### * Placing new orders 

> `POST /buy`

Example body:

```json 
	{
		"customer": {
			"id": "<SOME_CUSTOMER_ID>"
		},
		"giftcard":{
    		"price":"<PRICE_IN_USD>",
    		"description":"Amazon.com Gift Cards"
		}
    
	}
```
Response: 200 OK

```json
TBD
```

