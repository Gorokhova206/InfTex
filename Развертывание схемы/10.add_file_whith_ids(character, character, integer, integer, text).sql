-- Function: public.add_file_whith_ids(character, character, integer, integer, text)

-- DROP FUNCTION public.add_file_whith_ids(character, character, integer, integer, text);

CREATE OR REPLACE FUNCTION public.add_file_whith_ids(
    character,
    character,
    integer,
    integer,
    text)
  RETURNS SETOF file AS
$BODY$
BEGIN
	begin
		insert into file(name, description,extension_id,folder_id,content)
		values($1,$2,$3,$4,$5);
		raise notice 'Новый файл добавлен';
	exception when others then 
		raise notice 'Новый файл не добавлен';
	end;
	
END;
 
 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION public.add_file_whith_ids(character, character, integer, integer, text)
  OWNER TO postgres;
