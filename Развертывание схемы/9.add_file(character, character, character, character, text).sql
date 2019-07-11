-- Function: public.add_file(character, character, character, character, text)

-- DROP FUNCTION public.add_file(character, character, character, character, text);

CREATE OR REPLACE FUNCTION public.add_file(
    character,
    character,
    character,
    character,
    text)
  RETURNS SETOF file AS
$BODY$
DECLARE
	f integer;
	e integer;
BEGIN
	begin 
		select id
		into f
		from folder
		where name=$4;
	exception when no_data_found then
		f= null;
	end;
	begin
		select id
		into e
		from extension
		where type = $3;
	exception when no_data_found then
		e= null;
	end;
	if f is not null and e is not null then
		insert into file(name, description,extension_id,folder_id,content)
		values($1,$2,e,f,$5);
	end if;
	
END;
 
 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION public.add_file(character, character, character, character, text)
  OWNER TO postgres;
