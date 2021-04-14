import requests

# PyTest script for automated testing
# To test use "pytest --url 'https://localhost/someapipath' " and pytest should call both a 
# GET and POST to the URL to ensure successful status is returned along with PNG content.


def test_post(url):
    # Additional headers and payload required for POST
    headers = {'Content-Type': 'text/csv'}
    payload = 'this,is,a,test\n1,2,3,4\na,b,c,d\n5,6,7,8'

    # Post  
    respPost = requests.post(url, headers=headers, data=payload)

    print (respPost.text)

    # Check the status code
    assert respPost.status_code == 200




